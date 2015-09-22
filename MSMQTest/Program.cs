using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;

namespace MSMQTest
{
    class Program
    {
        static void Main( string[] args )
        {
            //MSMQTest.CreateMQ( "XUZZMSMQTEST" );
            //MSMQTest.SendMessage( "this is xuzz test " );

            MSMQTest.ReciveMessage();

            Console.Read();
        }
    }
    public class MSMQTest
    {
        public static MessageQueue CreateMQ( string strQPath )
        {
            try
            {
                string strPath = @".\private$\" + strQPath;

                if ( MessageQueue.Exists( strPath ) )
                {
                    return new MessageQueue( strPath );
                }

                return MessageQueue.Create( @".\private$\" + strQPath );
            }
            catch(MessageQueueException mqe)
            {
                Console.WriteLine( mqe.Message );
            }
            return null;
        }

        public static void SendMessage(string strMSG)
        {
            try
            {
                MessageQueue ms = CreateMQ( "xuzzmsmqtest" );
                Message msg = new Message();
                msg.Body = strMSG;
                msg.Formatter = new XmlMessageFormatter( new Type[] { typeof( string ) } );
                ms.Send( msg );
            }
            catch(ArgumentException e)
            {
                Console.WriteLine( e.Message );
            }
        }

        public static void ReciveMessage(  )
        {
            try
            {
                MessageQueue ms = CreateMQ( "xuzzmsmqtest" );
                ms.Formatter = new XmlMessageFormatter( new Type[] { typeof( string ) } );
                Message msg = ms.Receive();

                string strMSG = ( string ) msg.Body;

                Console.WriteLine( "Recive Msg:" + strMSG );
            }
            catch ( ArgumentException e )
            {
                Console.WriteLine( e.Message );
            }
        }
    }

    public class MSMQTranTest
    {
        public static MessageQueue CreateMQ( string strQPath )
        {
            try
            {
                string strPath = @".\private$\" + strQPath;

                if ( MessageQueue.Exists( strPath ) )
                {
                    return new MessageQueue( strPath );
                }

                return MessageQueue.Create( @".\private$\" + strQPath, true );
            }
            catch ( MessageQueueException mqe )
            {
                Console.WriteLine( mqe.Message );
            }
            return null;
        }

        public static void SendMessage( string strMSG )
        {
            try
            {
                MessageQueue mq = CreateMQ( "xuzzmsmqtest" );
                Message msg = new Message();
                msg.Body = strMSG;
                msg.Formatter = new XmlMessageFormatter( new Type[] { typeof( string ) } );

                MessageQueueTransaction mqt = new MessageQueueTransaction();
                mqt.Begin();
                //ms.Send( msg );
                mq.Send( msg, mqt );                
                mqt.Commit();
            }
            catch ( ArgumentException e )
            {
                Console.WriteLine( e.Message );
            }
        }

        public static void ReciveMessage()
        {
            try
            {
                MessageQueue ms = CreateMQ( "xuzzmsmqtest" );
                ms.Formatter = new XmlMessageFormatter( new Type[] { typeof( string ) } );
                Message msg = ms.Receive();
                
                string strMSG = ( string ) msg.Body;

                Console.WriteLine( "Recive Msg:" + strMSG );
            }
            catch ( ArgumentException e )
            {
                Console.WriteLine( e.Message );
            }
        }

    }
    
}

