using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenFTTH.CQRS
{
    public class InProcessCommandDispatcher : ICommandDispatcher
    {
        private IServiceProvider _serviceProvider;

        public InProcessCommandDispatcher(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public async Task<TResult> HandleAsync<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>
        {
            var service = this._serviceProvider.GetService(typeof(ICommandHandler<TCommand, TResult>)) as ICommandHandler<TCommand, TResult>;
            return await service.HandleAsync(command);
        }
    }
}
