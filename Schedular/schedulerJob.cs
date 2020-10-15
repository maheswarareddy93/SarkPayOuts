using System.Linq;
using Quartz;
using SarkPayOuts.Models.DbModels;
using System.Threading.Tasks;


namespace SarkPayOuts.Schedular
{
    [DisallowConcurrentExecution]
    public class schedulerJob: IJob
    {
        
        private readonly ApplicationDBContext _appontext;
        public schedulerJob(ApplicationDBContext appontext)
        {
            _appontext = appontext;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var agentDetailsCheck = (from agent in _appontext.AgentRegistration select agent).ToList();

            return Task.CompletedTask;
        }
    }
}
