using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBM.WMQ;

namespace MQLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            MQQueueManager queueManager;
            MQQueue queuePut;
            MQQueue queueGet;
            MQMessage queuePutMessage;
            MQMessage queueGetMessage;
            MQPutMessageOptions queuePutMessageOptions;
            MQGetMessageOptions queueGetMessageOptions;

            try
            {
                using (queueManager = new MQQueueManager("QM1", "DEV.APP.SVRCONN", "localhost(1414)"))
                {
                    using (queuePut = queueManager.AccessQueue("DEV.QUEUE.1", MQC.MQOO_OUTPUT + MQC.MQOO_FAIL_IF_QUIESCING))
                    {
                        queuePutMessage = new MQMessage { Format = MQC.MQFMT_STRING };
                        queuePutMessage.WriteString($"The time now: {DateTime.Now:O}");
                        queuePutMessageOptions = new MQPutMessageOptions();
                        queuePut.Put(queuePutMessage, queuePutMessageOptions);
                    }

                    using (queueGet = queueManager.AccessQueue("DEV.QUEUE.1", MQC.MQOO_INPUT_AS_Q_DEF + MQC.MQOO_FAIL_IF_QUIESCING))
                    {
                        queueGetMessage = new MQMessage { Format = MQC.MQFMT_STRING };
                        queueGetMessageOptions = new MQGetMessageOptions();
                        queueGet.Get(queueGetMessage, queueGetMessageOptions);

                        Console.WriteLine(queueGetMessage.ReadString(queueGetMessage.MessageLength));
                    }
                }
            }
            catch (MQException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}