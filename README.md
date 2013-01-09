MessengerAppender
=================

log4net appender to send log messages to a Windows Live account. Will be mainly obsolete
after March 15, 2013 as MS replaces it with Skype. *sigh*

Warning: project is in alpha state

Example config
--------------

    <log4net>
      <appender name="LiveAppender" type="MessengerAppender.LiveAppender, MessengerAppender">
        <username value="********@hotmail.com"/>
        <password value="********" />
      </appender>
      <root>
        <appender-ref ref="LiveAppender" />
      </root>
    </log4net>

