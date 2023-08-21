public static class SceneLoader
{
    public static void LoadScene(Define.Scenes kind)
    {
        GameManager.Instance.Clear();
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)kind);
    }
}
