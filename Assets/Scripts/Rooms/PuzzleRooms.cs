using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleRooms : MonoBehaviour
{
    private Helper Helper;
    public SimpleRoom Room;
    public DoorController DoorController;
    public EnemySpawner EnemySpawner;
    public List<string> TileColours = new List<string>() { "Red", "Green", "Blue", "Teal" };
    public List<string> RandomisedUnlockCode;
    public List<string> SubmittedCode = new List<string>();

    public Rune RedRune;
    public Rune BlueRune;
    public Rune GreenRune;
    public Rune TealRune;

    void Start()
    {
        Helper = GameObject.FindGameObjectWithTag("Helper").GetComponent<Helper>();
        var rnd = new System.Random();
        RandomisedUnlockCode = TileColours.OrderBy(item => rnd.Next()).ToList();

        Room = GetComponent<SimpleRoom>();

        DoorController = gameObject.transform.Find("DoorController").GetComponent<DoorController>();
        DoorController.openCondition = DoorController.OpenCondition.PuzzleComplete;

        var roomCenter = Room.transform.Find("RoomCenter").transform.position;
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
        foreach (var tile in Room.pillarTiles)
        {

            GameObject wall = Instantiate<GameObject>(
                Helper.Wall,
                tile.transform.position,
                Quaternion.identity);

            wall.transform.parent = tilesObject;
            Room.RemoveSpawnableTile(wall.transform.localPosition);

            // Flame bowls and arrow traps
            GameObject flameBowl = Instantiate(
                Helper.Flamebowl,
                tile.transform.position,
                Quaternion.identity);

            flameBowl.transform.parent = tilesObject;

            if (flameBowl.transform.localPosition == new Vector3(-0.25f, 0.25f, 0))
            {
                flameBowl.name = "flameBowl1";

                GameObject arrowTrap = Instantiate(
                    Helper.ArrowTrap,
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
                    Helper.ArrowTrap,
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
                    Helper.ArrowTrap,
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
                    Helper.ArrowTrap,
                    GetComponent<SimpleRoom>().arrowTrap4Position.transform.position,
                    Quaternion.identity);

                arrowTrap.transform.parent = tilesObject;
                arrowTrap.name = "arrowTrap4";
                arrowTrap.GetComponent<ArrowTrap>().shootLeft = true;

                arrowTrap.transform.rotation *= Quaternion.AngleAxis(-90, transform.forward);
                arrowTrap.transform.position += new Vector3(-0.4f, 0f, 0f);
            }
            Room.AddItemToRoomContents(tile.transform.localPosition, 'P');
        }

        gameObject.AddComponent<EnemySpawner>();
        EnemySpawner = GetComponent<EnemySpawner>();
        EnemySpawner.SetEnemyCount(UnityEngine.Random.Range(0, 3));

        // -Runes - randomised placement
        List<GameObject> runes = new List<GameObject>()
            {
                Resources.Load<GameObject>("BlueTile"),
                Resources.Load<GameObject>("GreenTile"),
                Resources.Load<GameObject>("RedTile"),
                Resources.Load<GameObject>("TealTile")
            };

        foreach (var rune in Room.runeTiles)
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

        RedRune = tilesObject.Find("RedTile(Clone)").gameObject.GetComponent<Rune>();
        BlueRune = tilesObject.Find("BlueTile(Clone)").gameObject.GetComponent<Rune>(); ;
        GreenRune = tilesObject.Find("GreenTile(Clone)").gameObject.GetComponent<Rune>(); ;
        TealRune = tilesObject.Find("TealTile(Clone)").gameObject.GetComponent<Rune>(); ;
    }

    public void SubmitCode(string code)
    {
        // gate to stop same rune being submitted twice
        if (!SubmittedCode.Contains(code))
        {
            SubmittedCode.Add(code);

            if (RandomisedUnlockCode[SubmittedCode.Count - 1] == code)
            {
                Helper.AudioManager.PlayAudioClip("RuneSuccess");
                Debug.Log("correct"); // For Debug, perhaps add some graphical effect instead?
            }
            else
            {
                Helper.AudioManager.PlayAudioClip("RuneFailure");
                Debug.Log("false"); // For Debug, perhaps add some graphical effect instead?
            }
        }
    }

    void Update()
    {
        // Code is right
        if (SubmittedCode.Count == RandomisedUnlockCode.Count() && RandomisedUnlockCode.SequenceEqual(SubmittedCode))
        {
            DoorController.roomComplete = true;
        }

        // Code is wrong
        if (SubmittedCode.Count == RandomisedUnlockCode.Count() && !RandomisedUnlockCode.SequenceEqual(SubmittedCode))
        {
            switch (SubmittedCode[SubmittedCode.Count - 1])
            {
                case "Red":
                    RedRune.myTrap.GetComponent<ArrowTrap>().ActivateOnce();
                    break;

                case "Green":
                    GreenRune.myTrap.GetComponent<ArrowTrap>().ActivateOnce();
                    break;

                case "Blue":
                    BlueRune.myTrap.GetComponent<ArrowTrap>().ActivateOnce();
                    break;

                case "Teal":
                    TealRune.myTrap.GetComponent<ArrowTrap>().ActivateOnce();
                    break;
            }

            RedRune.ResetRune();
            RedRune.flameBowl.GetComponent<FlameBowl>().Extinguish();
            GreenRune.ResetRune();
            GreenRune.flameBowl.GetComponent<FlameBowl>().Extinguish();
            BlueRune.ResetRune();
            BlueRune.flameBowl.GetComponent<FlameBowl>().Extinguish();
            TealRune.ResetRune();
            TealRune.flameBowl.GetComponent<FlameBowl>().Extinguish();
            SubmittedCode = new List<string>();
        }
    }
}