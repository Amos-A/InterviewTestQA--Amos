using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using static Amazon.Internal.RegionEndpointProviderV2;

namespace InterviewTestQA.InterviewTestAutomation
{
    public class SQSQueueTest
    {
        [Fact]
        public void Test1()
        {

            //=========================  Send message =========================================================================//

            var queueUrl = "https://sqs.eu-north-1.amazonaws.com/183320071121/MySQSQueue";

            Console.WriteLine("Creating a SQS Client and connecting ");

            String accesskey = dcryptXOR("CCU9JCEgJTNFHjU6QAYvMC03MSU=", "InterviewTest");
            String secretKey = dcryptXOR("DARMVQsdURQzPzwlOAU7DQsxJiEfRR1XBV8KXTgGAjkaXCIiUiYWLg==", "InterviewTest");

            var aws_Credentialss = new BasicAWSCredentials(accesskey, secretKey);



            var awsSQSClient = new AmazonSQSClient(aws_Credentialss, Amazon.RegionEndpoint.EUNorth1);

            var sendMessageReq = new SendMessageRequest();
            sendMessageReq.QueueUrl = queueUrl;
            sendMessageReq.MessageBody = "{\r\n\"user\":\"Thomas P\"\r\n\"role\":\"Business Analyst\"\r\n}";

            Console.WriteLine("Sending Message");
            var sendMessageResponse = awsSQSClient.SendMessageAsync(sendMessageReq).Result;

            if (sendMessageResponse.HttpStatusCode.Equals(System.Net.HttpStatusCode.OK))
            {
                Console.WriteLine("Message sent successfully! \n");
                Console.WriteLine("Message is returned is: " + sendMessageResponse.MessageId);
            }
            else
            {
                Assert.False(true, "Message sent failed! \n");
                //Console.WriteLine("Message sent failed! \n");
            }


            //=========================  Receive message =========================================================================//

            var receiveMessageReq = new ReceiveMessageRequest();
            receiveMessageReq.QueueUrl = queueUrl;
            var response = awsSQSClient.ReceiveMessageAsync(receiveMessageReq).Result;

            bool messageFound = false;
            if (response.Messages.Any())
            {
                foreach (var message in response.Messages)
                {
                    if (message.MessageId.Equals(sendMessageResponse.MessageId))
                    {
                        Console.WriteLine("Send message found in queue: \n" + message.Body);
                        messageFound = true;
                        var deleteMessageRequest = new DeleteMessageRequest();
                        deleteMessageRequest.QueueUrl = queueUrl;
                        deleteMessageRequest.ReceiptHandle = message.ReceiptHandle;
                    }
                }
            }

            if (!messageFound)
            {
                Assert.False(true, "\nSent message not found in queue!");
                //Console.WriteLine("\nSent message not found in queue!");
            }
            if (!response.Messages.Any())
            {
                Assert.False(true, "\nNo messages in queue!");
                //Console.WriteLine("\nNo messages in queue!");
            }
        }


        //------------------  Decrypt the keys -----------------------------------------------------//

        public static String dcryptXOR(string message, string key)
        {

            try
            {
                if (message == null || key == null)
                    return null;
                char[] keys = key.ToCharArray();
                char[] mesg = Encoding.UTF8.GetChars(Convert.FromBase64String(message));
                int ml = mesg.Length;
                int kl = keys.Length;
                char[] newmsg = new char[ml];
                for (int i = 0; i < ml; i++)
                {
                    newmsg[i] = (char)(mesg[i] ^ keys[i % kl]);
                }
                mesg = null;
                keys = null;
                return new String(newmsg);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException occured: " + e.Message);
                return null;
            }

        }
    }
}
