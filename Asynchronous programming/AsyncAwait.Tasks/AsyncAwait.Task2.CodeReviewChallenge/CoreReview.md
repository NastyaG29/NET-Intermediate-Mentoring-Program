## Code Review Remarks

1.  В методе ```Privacy()``` исполнение свойства ```Result``` блокирует поток до тех пор, пока результат не будет получен. Для избежания блокировки потока сделаем метод асинхронным. 
Еще одна потенциальная проблема: возможно возникловения deadlock.
    **Было**

        public ActionResult Privacy()
        {
            ViewBag.Message = _privacyDataService.GetPrivacyDataAsync().Result;
            return View();
        }
        
    **Стало**

        public async Task<ActionResult> Privacy()
        {
            ViewBag.Message = await _privacyDataService.GetPrivacyDataAsync();
            return View();
        }

2. В методе контроллера ```Help()``` не требуется вызов ```ConfigureAwait(false)```.
    Из msdn: 
    > You should not use ConfigureAwait when you have code after the await in the method that needs the context. For GUI apps, this includes any code that manipulates GUI elements, writes data-bound properties or depends on a GUI-specific type such as Dispatcher/CoreDispatcher. For ASP.NET apps, this includes any code that uses HttpContext.Current or builds an ASP.NET response, including return statements in controller actions.
        
    **Было**

        ViewBag.RequestInfo = await _assistant.RequestAssistanceAsync("guest").ConfigureAwait(false);

    **Стало**
    
        ViewBag.RequestInfo = await _assistant.RequestAssistanceAsync("guest");

3. Класс ```StatisticMiddleware```. 
    - В методе ```UpdateHeaders``` нужно убрать конструкцию ```GetAwaiter().GetResult()``` из-за блокировки вызывающего потока (так называемая **Sync over Async** проблема).
    - Сам метод ```GetVisitsCountAsync``` следует вызывать асинхронно. 
    - В случае вызова ```RegisterVisitAsync``` конструкция ```ConfigureAwait(false)``` не имеет смысла, так как выполенение происходит в ```ThreadPool``` из-за использования ```Task.Run``` и таким образом текущей контекст синхронизации будет равен ```null```.
    - По поводу вызова ```GetAwaiter``` в документации есть ремарка: 
        > This method is intended for compiler use rather than for use in application code.
    
    **Было**

        Task staticRegTask = Task.Run(() => _statisticService.RegisterVisitAsync(path).ConfigureAwait(false).GetAwaiter().OnCompleted(UpdateHeaders));

        void UpdateHeaders()
        {
            context.Response.Headers.Add(CustomHttpHeaders.TotalPageVisits,
                    _statisticService.GetVisitsCountAsync(path).GetAwaiter().GetResult().ToString);
        }

    **Стало**       
        
        await _statisticService.RegisterVisitAsync(path);
        var visitors = await _statisticService.GetVisitsCountAsync(path);
        context.Response.Headers.Add(CustomHttpHeaders.TotalPageVisits, visitors.ToString());

4. Класс ```ManualAssistant```
    - В методе ```RequestAssistanceAsync``` вместо ```Thread.Sleep(5000)``` нужно использовать асинхронный вызов. Судя по комментарию, остановка потока используется для того, чтобы дождаться выполнения ```RegisterSupportRequestAsync```. А откуда мы знаем, что этого времени хватит? 
    - После получения ошибки результат последующей операции, по сути, уже известен. Можно просто вызвать ```Task.FromResut()```;
    - 
    **Было**

        try
        {
            Task t = _supportService.RegisterSupportRequestAsync(requestInfo);
            Console.WriteLine(t.Status); // this is for debugging purposes
            Thread.Sleep(5000); // this is just to be sure that the request is registered
            return await _supportService.GetSupportInfoAsync(requestInfo).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            return await Task.Run(async () => await Task.FromResult($"Failed to register assistance request. Please try later. {ex.Message}"));
        }

    **Стало**
    
        try
        {
            await _supportService.RegisterSupportRequestAsync(requestInfo);
            return await _supportService.GetSupportInfoAsync(requestInfo).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            return await Task.FromResult($"Failed to register assistance request. Please try later. {ex.Message}");
        }

5. В методе ```GetPrivacyDataAsync``` интерфейса ```IPrivacyDataService``` возврещаемый тип можно сделать ```ValueTask<string>```.