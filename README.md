MessengerAppender
=================

log4net appender to send log messages to a Windows Live account

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

