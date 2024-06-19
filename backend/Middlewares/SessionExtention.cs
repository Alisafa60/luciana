public static class SessionExtention{
    private const string SessionKey = "SessionId";

    public static string GetOrCreateSessionId(this ISession session) {
        var sessionId = session.GetString(SessionKey);
        if (string.IsNullOrEmpty(sessionId)) {
            sessionId = Guid.NewGuid().ToString();
            session.SetString(SessionKey, sessionId);
        }

        return sessionId;
    }
}