using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tube.Specs
{
    [TaskName("fake task")]
    [TaskDependsOn("task1", "task2")]
    public class FakeTask : Task<FakeTaskContext>
    {
        public override void Execute(FakeTaskContext context)
        {
            this.Executed = context;
        }

        public FakeTaskContext Executed { get; private set; }
    }

    public class FakeMessage
    {
    }

    [TaskName("simple fake task")]
    public class SimpleFakeTask : Task<FakeTaskContext>
    {
        public override void Execute(FakeTaskContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class NamelessFakeTask : Task<FakeTaskContext>
    {
        public override void Execute(FakeTaskContext context)
        {
            throw new NotImplementedException();
        }
    }
}
