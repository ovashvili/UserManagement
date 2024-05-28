namespace UserManagement.Domain.Exceptions;

public class UserIsAlreadyAuthenticatedException(string message) : Exception(message);