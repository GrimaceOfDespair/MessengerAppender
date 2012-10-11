using System;
using System.Collections.Generic;
using System.Text;
using MSNPSharp;
using log4net.Appender;
using log4net.Core;
using log4net.Util;

namespace MessengerAppender
{
  public class LiveAppender : AppenderSkeleton
  {
    private Messenger _messenger;
    private readonly Dictionary<long, Contact> _contacts = new Dictionary<long, Contact>();

    public string Username { get; set; }
    public string Password { get; set; }

    public IEnumerable<Contact> Contacts
    {
      get { return _contacts.Values; }
    }

    public Messenger Messenger
    {
      get { return _messenger; }
    }

    public override void ActivateOptions()
    {
      base.ActivateOptions();

      _messenger = new Messenger
                     {
                       Credentials = new Credentials(Username, Password, MsnProtocol.MSNP21),
                     };

      _messenger.ConnectingException += (sender, args) =>
                                          {
                                            if (LogLog.IsErrorEnabled)
                                            {
                                              LogLog.Error("Connection error", args.Exception);
                                            }
                                          };
      _messenger.Nameserver.ExceptionOccurred += (sender, args) =>
                                                   {
                                            if (LogLog.IsErrorEnabled)
                                            {
                                              LogLog.Error("General error", args.Exception);
                                            }
                                                   };

      _messenger.Nameserver.AuthenticationError += (sender, args) =>
                                                     {
                                            if (LogLog.IsErrorEnabled)
                                            {
                                              LogLog.Error("Authentication error", args.Exception);
                                            }
                                                     };

      _messenger.Nameserver.ServerErrorReceived+= (sender, args) =>
                                                    {
                                                      if (LogLog.IsErrorEnabled)
                                                      {
                                                        LogLog.Error("Server error: " + args.Description +
                                                                     "\n(MSN Error:" +
                                                                     args.MSNError + ")");
                                                      }
                                                    };

      _messenger.Nameserver.ContactOnline += (sender, args) =>
                                               {
                                                 lock (_contacts)
                                                 {
                                                   if (LogLog.IsDebugEnabled)
                                                   {
                                                     LogLog.Debug("Contact " + args.Contact.Account + " has come online");
                                                   }

                                                   _contacts[args.Contact.CID] = args.Contact;
                                                 }
                                               };
      _messenger.Nameserver.ContactOnline += (sender, args) =>
                                               {
                                                 lock (_contacts)
                                                 {
                                                   if (LogLog.IsDebugEnabled)
                                                   {
                                                     LogLog.Debug("Contact " + args.Contact.Account + " has gone offline");
                                                   }

                                                   var contactId = args.Contact.CID;
                                                   Contact contact;
                                                   if (_contacts.TryGetValue(contactId, out contact))
                                                   {
                                                     _contacts.Remove(contactId);
                                                   }
                                                 }
                                               };

      _messenger.ContactService.FriendshipRequested +=
        (sender, args) => _messenger.ContactService.AddNewContact(args.Contact.Account);

      _messenger.Connect();
    }

    protected override void OnClose()
    {
      if (_messenger != null && _messenger.Connected)
      {
        _messenger.Disconnect();
      }

      base.OnClose();
    }

    protected override void Append(LoggingEvent loggingEvent)
    {
      var message = loggingEvent.RenderedMessage;

      MakeSureMessengerIsConnected();

      lock (_contacts)
      {
        foreach (var contact in _contacts.Values)
        {
          _messenger.SendTextMessage(contact, message);
        }
      }
    }

    private void MakeSureMessengerIsConnected()
    {
      if (_messenger.Connected == false)
      {
        if (LogLog.IsWarnEnabled)
        {
          LogLog.Warn("LiveAppender was not connected. Reconnecting.");
        }

        lock (_contacts)
        {
          _contacts.Clear();
        }
        _messenger.Connect();
      }
    }
  }
}
