namespace WowMultiBox.Core;

public enum CtrlEvents
{
    CTRL_C_EVENT =
        0, // 	A CTRL+C signal was received, either from keyboard input or from a signal generated by the GenerateConsoleCtrlEvent function.

    CTRL_BREAK_EVENT =
        1, //  A CTRL+BREAK signal was received, either from keyboard input or from a signal generated by GenerateConsoleCtrlEvent.

    CTRL_CLOSE_EVENT =
        2, //  A signal that the system sends to all processes attached to a console when the user closes the console (either by clicking Close on the console window's window menu, or by clicking the End Task button command from Task Manager).

    CTRL_LOGOFF_EVENT =
        5, //  A signal that the system sends to all console processes when a user is logging off. This signal does not indicate which user is logging off, so no assumptions can be made. Note that this signal is received only by services. Interactive applications are terminated at logoff, so they are not present when the system sends this signal.

    CTRL_SHUTDOWN_EVENT =
        6 // A signal that the system sends when the system is shutting down. Interactive applications are not present by the time the system sends this signal, therefore it can be received only be services in this situation. Services also have their own notification mechanism for shutdown events. For more information, see Handler. This signal can also be generated by an application using GenerateConsoleCtrlEvent.
}