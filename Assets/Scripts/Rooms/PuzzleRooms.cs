using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleRooms : MonoBehaviour
{
    private Helper helper;
    public SimpleRoom room;
    public DoorController doorController;
    public EnemySpawner enemySpawner;
    public List<string> tileColours = new List<string>() { "Red", "Green", "Blue", "Teal" };
    public List<string> randomisedUnlockCode;
    public List<string> submittedCode = new List<string>();

    public Rune redRune;
    public Rune blueRune;
    public Rune greenRune;
    public Rune tealRune;


    void Start()
    {
        helper = GameObject.FindGameObjectWithTag("Helper").GetComponent<Helper>();
        var rnd = new System.Random();
        randomisedUnlockCode = tileColours.OrderBy(item => rnd.Next()).ToList();

        gameObject.AddComponent<EnemySpawner>();
        enemySpawner = GetComponent<EnemySpawner>();
        enemySpawner.SetEnemyCount(UnityEngine.Random.Range(0, 3));

        room = GetComponent<SimpleRoom>();

        doorController = gameObject.transform.Find("DoorController").GetComponent<DoorController>();
        doorController.openCondition = DoorController.OpenCondition.PuzzleComplete;

        var roomCenter = room.transform.Find("RoomCenter").transform.position;
        var tilesObject = transform.Find("Tiles");

        // -Chest
        GameObject chest = Instantiate(Resources.Load("newChest") as GameObject, roomCenter, Quaternion.identity);
        chest.name = "chest";
        chest.transform.parent = tilesObject;

        // Barrier
        GameObject barrier = Instantiate(
            Resources.Load<GameObject>("chestBarrier"),
            roomCenter,
            Quaternion.identity);
        barrier.transform.parent = tilesObject;

        // Pillars
        foreach (var tile in room.pillarTiles)
        {
            GameObject wall = Instantiate<GameObject>(
                helper.wall,
                tile.transform.position,
                Quaternion.identity);
            
            wall.transform.parent = tilesObject;

            // Flame bowls and arrow traps
            GameObject flameBowl = Instantiate(
                helper.flameBowl,
                tile.transform.position,
                Quaternion.identity);

            flameBowl.transform.parent = tilesObject;

            if (flameBowl.transform.localPosition == new Vector3(-0.25f, 0.25f, 0))
            {
                flameBowl.name = "flameBowl1";

                GameObject arrowTrap = Instantiate(
                    helper.arrowTrap,
                    GetComponent<SimpleRoom>().arrowTrap1Position.transform.position,
                    Quaternion.identity);

                arrowTrap.transform.parent = tilesObject;
                arrowTrap.name = "arrowTrap1";
                arrowTrap.GetComponent<ArrowTrap>().shootRight = true;

                arrowTrap.transform.rotation *= Quaternion.AngleAxis(90, transform.forward);
                arrowTrap.transform.position += new Vector3(0.4f, 0f, 0f);
            }

            if (flameBowl.transform.localPosition == new Vector3(0.25f, 0.25f, 0))
            {
                flameBowl.name = "flameBowl2";

                GameObject arrowTrap = Instantiate(
                    helper.arrowTrap,
                    GetComponent<SimpleRoom>().arrowTrap2Position.transform.position,
                    Quaternion.identity);

                arrowTrap.transform.parent = tilesObject;
                arrowTrap.name = "arrowTrap2";
                arrowTrap.GetComponent<ArrowTrap>().shootLeft = true;

                arrowTrap.transform.rotation *= Quaternion.AngleAxis(-90, transform.forward);
                arrowTrap.transform.position += new Vector3(-0.4f, 0f, 0f);
            }

            if (flameBowl.transform.localPosition == new Vector3(-0.25f, -0.15f, 0))
            {
                flameBowl.name = "flameBowl3";
                GameObject arrowTrap = Instantiate(
                    helper.arrowTrap,
                    GetComponent<SimpleRoom>().arrowTrap3Position.transform.position,
                    Quaternion.identity);

                arrowTrap.transform.parent = tilesObject;
                arrowTrap.name = "arrowTrap3";
                arrowTrap.GetComponent<ArrowTrap>().shootRight = true;

                arrowTrap.transform.rotation *= Quaternion.AngleAxis(90, transform.forward);
                arrowTrap.transform.position += new Vector3(0.4f, 0f, 0f);
            }


            if (flameBowl.transform.localPosition == new Vector3(0.25f, -0.15f, 0))
            {
                flameBowl.name = "flameBowl4";
                GameObject arrowTrap = Instantiate(
                    helper.arrowTrap,
                    GetComponent<SimpleRoom>().arrowTrap4Position.transform.position,
                    Quaternion.identity);

                arrowTrap.transform.parent = tilesObject;
                arrowTrap.name = "arrowTrap4";
                arrowTrap.GetComponent<ArrowTrap>().shootLeft = true;

                arrowTrap.transform.rotation *= Quaternion.AngleAxis(-90, transform.forward);
                arrowTrap.transform.position += new Vector3(-0.4f, 0f, 0f);
            }
            room.AddItemToRoomContents(tile.transform.localPosition, 'P');
        }

        // -Runes - randomised placement
        List<GameObject> runes = new List<GameObject>()
            {
                Resources.Load<GameObject>("BlueTile"),
                Resources.Load<GameObject>("GreenTile"),
                Resources.Load<GameObject>("RedTile"),
                Resources.Load<GameObject>("TealTile")
            };

        foreach (var rune in room.runeTiles)
        {
            var r = UnityEngine.Random.Range(0, runes.Count);
            var randomRune = runes[r];

            GameObject _rune = Instantiate(
                randomRune,
                rune.transform.position,
                Quaternion.identity);

            _rune.transform.parent = tilesObject;
            runes.RemoveAt(r);

            if (_rune.transform.localPosition == new Vector3(-0.25f, 0.15f, 0))
            {
                _rune.GetComponent<Rune>().flameBowl = _rune.transform.parent.Find("flameBowl1").GetComponent<FlameBowl>();
                _rune.GetComponent<Rune>().myTrap = _rune.transform.parent.Find("arrowTrap1").gameObject;
            }

            if (_rune.transform.localPosition == new Vector3(0.25f, 0.15f, 0))
            {
                _rune.GetComponent<Rune>().flameBowl = _rune.transform.parent.Find("flameBowl2").GetComponent<FlameBowl>();
                _rune.GetComponent<Rune>().myTrap = _rune.transform.parent.Find("arrowTrap2").gameObject;
            }

            if (_rune.transform.localPosition == new Vector3(-0.25f, -0.25f, 0))
            {
                _rune.GetComponent<Rune>().flameBowl = _rune.transform.parent.Find("flameBowl3").GetComponent<FlameBowl>();
                _rune.GetComponent<Rune>().myTrap = _rune.transform.parent.Find("arrowTrap3").gameObject;
            }

            if (_rune.transform.localPosition == new Vector3(0.25f, -0.25f, 0))
            {
                _rune.GetComponent<Rune>().flameBowl = _rune.transform.parent.Find("flameBowl4").GetComponent<FlameBowl>();
                _rune.GetComponent<Rune>().myTrap = _rune.transform.parent.Find("arrowTrap4").gameObject;
            }

        }

        redRune = tilesObject.Find("RedTile(Clone)").gameObject.GetComponent<Rune>();
        blueRune = tilesObject.Find("BlueTile(Clone)").gameObject.GetComponent<Rune>(); ;
        greenRune = tilesObject.Find("GreenTile(Clone)").gameObject.GetComponent<Rune>(); ;
        tealRune = tilesObject.Find("TealTile(Clone)").gameObject.GetComponent<Rune>(); ;
    }

    public void SubmitCode(string code)
    {
        // gate to stop same rune being submitted twice
        if (!submittedCode.Contains(code))
        {
            submittedCode.Add(code);

            if (randomisedUnlockCode[submittedCode.Count - 1] == code)
            {
                helper.AudioManager.PlayAudioClip("RuneSuccess");
                Debug.Log("correct"); // For Debug, perhaps add some graphical effect instead?
            }
            else
            {
                helper.AudioManager.PlayAudioClip("RuneFailure");
                Debug.Log("false"); // For Debug, perhaps add some graphical effect instead?
            }
        }
    }

    void Update()
    {
        // Code is right
        if (submittedCode.Count == randomisedUnlockCode.Count() && randomisedUnlockCode.SequenceEqual(submittedCode))
        {
            doorController.roomComplete = true;
        }

        // Code is wrong
        if (submittedCode.Count == randomisedUnlockCode.Count() && !randomisedUnlockCode.SequenceEqual(submittedCode))
        {
            switch (submittedCode[submittedCode.Count - 1])
            {
                case "Red":
                    redRune.myTrap.GetComponent<ArrowTrap>().ActivateOnce();
                    break;

                case "Green":
                    greenRune.myTrap.GetComponent<ArrowTrap>().ActivateOnce();
                    break;

                case "Blue":
                    blueRune.myTrap.GetComponent<ArrowTrap>().ActivateOnce();
                    break;

                case "Teal":
                    tealRune.myTrap.GetComponent<ArrowTrap>().ActivateOnce();
                    break;
            }

            redRune.ResetRune();
            redRune.flameBowl.GetComponent<FlameBowl>().Extinguish();
            greenRune.ResetRune();
            greenRune.flameBowl.GetComponent<FlameBowl>().Extinguish();
            blueRune.ResetRune();
            blueRune.flameBowl.GetComponent<FlameBowl>().Extinguish();
            tealRune.ResetRune();
            tealRune.flameBowl.GetComponent<FlameBowl>().Extinguish();
            submittedCode = new List<string>();
        }
    }
}