using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using ZPackage;

public class Tutorial : MonoBehaviour
{
    GameObject jumpTutorialUI;
    GameObject RollTutorialUI;
    GameObject tutorCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        await LoadAssets();
        StartCoroutine(LocalCor());

        IEnumerator LocalCor()
        {
            Debug.Log("waiting play");
            yield return new WaitUntil(() => Z.GM.State == GameState.Playing);
            Debug.Log("waiting level ins");
            yield return new WaitUntil(() => transform.Find("Sections/Section").childCount > 1);

            Transform target = transform.GetChild(0).GetChild(0).GetChild(1);
            while (target.position.z - Z.Player.transform.position.z > 1)
            {
                Debug.Log("waiting player reach");
                yield return null;
            }
            yield return JumpTutorial();
            yield return new WaitUntil(() => transform.Find("Sections/Section").childCount > 2);
            target = transform.GetChild(0).GetChild(0).GetChild(2);

            while (target.position.z - Z.Player.transform.position.z > 1)
            {
                yield return null;
            }
            yield return SlideTutorial();

            Destroy(this);
        }
    }
    public async Task LoadAssets()
    {
        // var handle = Addressables.LoadAssetAsync<GameObject>("Tutor");
        var handle = Addressables.InstantiateAsync("Tutor");
        // var handle2 = Addressables.LoadAssetAsync<GameObject>("rollUI");
        // await Task.WhenAll(handle.Task, handle2.Task);
        await handle.Task;
        tutorCanvas = handle.Result;
        // RollTutorialUI = handle2.Result;
    }
    public Coroutine JumpTutorial()
    {
        Time.timeScale = 0;
        jumpTutorialUI = tutorCanvas.transform.GetChild(0).GetChild(0).gameObject;
        jumpTutorialUI?.gameObject.SetActive(true);
        Animation animation = tutorCanvas.GetComponent<Animation>();
        animation.Play("DragUp");
        return StartCoroutine(LocalCor());
        IEnumerator LocalCor()
        {

            while (!SwipeAndPinch.UpDrag())
            {
                Debug.Log("waiting player drag up");
                animation["DragUp"].time += Time.unscaledDeltaTime;
                yield return null;
            }
            jumpTutorialUI?.gameObject.SetActive(false);
            Time.timeScale = 1;
            tutorCanvas.GetComponent<Animation>().Stop();
        }
    }
    public Coroutine SlideTutorial()
    {
        Time.timeScale = 0;
        RollTutorialUI = tutorCanvas.transform.GetChild(0).GetChild(0).gameObject;
        RollTutorialUI?.gameObject.SetActive(true);
        Animation animation = tutorCanvas.GetComponent<Animation>();
        animation.Play("DragDown");
        return StartCoroutine(LocalCor());
        IEnumerator LocalCor()
        {

            while (!SwipeAndPinch.DownDrag())
            {
                Debug.Log("waiting player drag down");
                animation["DragDown"].time += Time.unscaledDeltaTime;
                yield return null;
            }
            RollTutorialUI?.gameObject.SetActive(false);
            Time.timeScale = 1;
            tutorCanvas.GetComponent<Animation>().Stop();
        }
    }


}
