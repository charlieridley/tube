using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tube.Specs
{
    [TaskName("task1")]
    public class TaskOrdererTask1 : Task<FakeTaskContext>
    {
        public override void Execute(FakeTaskContext context)
        {
            
        }
    }

    [TaskName("task2")]
    [TaskDependsOn("task1")]
    public class TaskOrdererTask2 : Task<FakeTaskContext>
    {
        public override void Execute(FakeTaskContext context)
        {

        }
    }

    [TaskName("task3")]
    [TaskDependsOn("task2")]
    public class TaskOrdererTask3 : Task<FakeTaskContext>
    {
        public override void Execute(FakeTaskContext context)
        {

        }
    }

    [TaskName("task4")]
    [TaskDependsOn("task1", "task2")]
    public class TaskOrdererTask4 : Task<FakeTaskContext>
    {
        public override void Execute(FakeTaskContext context)
        {

        }
    }

    [TaskName("task5")]
    [TaskDependsOn("task6")]
    public class TaskOrdererTask5 : Task<FakeTaskContext>
    {
        public override void Execute(FakeTaskContext context)
        {

        }
    }

    [TaskName("task6")]
    [TaskDependsOn("task5")]
    public class TaskOrdererTask6 : Task<FakeTaskContext>
    {
        public override void Execute(FakeTaskContext context)
        {

        }
    }

    [TaskName("task7")]
    [TaskDependsOn("task8")]
    public class TaskOrdererTask7 : Task<FakeTaskContext>
    {
        public override void Execute(FakeTaskContext context)
        {

        }
    }

    [TaskName("task8")]
    [TaskDependsOn("task9")]
    public class TaskOrdererTask8 : Task<FakeTaskContext>
    {
        public override void Execute(FakeTaskContext context)
        {

        }
    }

    [TaskName("task9")]
    [TaskDependsOn("task7")]
    public class TaskOrdererTask9 : Task<FakeTaskContext>
    {
        public override void Execute(FakeTaskContext context)
        {

        }
    }
}
