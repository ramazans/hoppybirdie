using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLooper : MonoBehaviour {
    
    public float speed = 1f;

    //Background
    public GameObject background1;
    public GameObject background2;
    Rigidbody2D background1Rb;
    Rigidbody2D background2Rb;
    float bgWidth = 0;
    
    //Block
    public GameObject block;
    int blockCount = 8;
    GameObject[] blocks;
    int blockIndex = 0;
    float blockTiming = 0f;
    float[] blockTimingRange = { 2f, 3.5f };
    float blockRandomTiming = 0;
    BlockGenerator blockGenerator;

    //Cloud
    public GameObject cloud;
    int cloudCount = 8;
    GameObject[] clouds;
    int cloudIndex = 0;
    float cloudTiming = 0;
    float[] cloudTimingRange = { 2f, 7f};
    float cloudRandomTiming = 0;
    float[] cloudRandomRangeY = { -3.2f, 1.7f };
    float cloudRandomY = 0;

    //Cactus
    public GameObject cactus;
    int cactusCount = 13;
    GameObject[] cactuses;
    int cactusIndex = 0;
    float cactusTiming = 0f;
    float[] cactusTimingRange = { 0.5f, 3.5f };
    float cactusRandomTiming = 0;

    //Static Objects
    float destroyTiming = 0f;

    public GameObject tree;
    Rigidbody2D treeRb;

    public GameObject block1;
    public GameObject block2;
    public GameObject block3;
    Rigidbody2D block1Rb;
    Rigidbody2D block2Rb;
    Rigidbody2D block3Rb;

    public GameObject cactus1;
    public GameObject cactus2;
    public GameObject cactus3;
    public GameObject cactus4;
    public GameObject cactus5;
    public GameObject cactus6;
    Rigidbody2D cactus1Rb;
    Rigidbody2D cactus2Rb;
    Rigidbody2D cactus3Rb;
    Rigidbody2D cactus4Rb;
    Rigidbody2D cactus5Rb;
    Rigidbody2D cactus6Rb;

    public GameObject cloud1;
    public GameObject cloud2;
    public GameObject cloud3;
    public GameObject cloud4;
    public GameObject cloud5;
    Rigidbody2D cloud1Rb;
    Rigidbody2D cloud2Rb;
    Rigidbody2D cloud3Rb;
    Rigidbody2D cloud4Rb;
    Rigidbody2D cloud5Rb;

    float stopTiming = 0f;
    bool stopScene = false;

    public void Start () {
        
        //Background
        background1Rb = background1.GetComponent<Rigidbody2D>();
        background2Rb = background2.GetComponent<Rigidbody2D>();
        background1Rb.velocity = new Vector2(-speed, 0);
        background2Rb.velocity = new Vector2(-speed, 0);
        bgWidth = background1Rb.GetComponent<BoxCollider2D>().size.x;
        
        //Block
        blocks = new GameObject[blockCount];
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i] = Instantiate(block, new Vector2(0, -4.07f), Quaternion.identity);
            blocks[i].SetActive(true);
            Rigidbody2D blockRb = blocks[i].GetComponent<Rigidbody2D>();
            blockRb.velocity = new Vector2(-speed, 0);
        }

        //Cloud
        clouds = new GameObject[cloudCount];
        for (int i = 0; i < clouds.Length; i++)
        {
            clouds[i] = Instantiate(cloud, new Vector2(0, 0), Quaternion.identity);
            clouds[i].SetActive(true);
            Rigidbody2D cloudRb = clouds[i].GetComponent<Rigidbody2D>();
            cloudRb.velocity = new Vector2(-speed / 1.5f, 0);
        }

        //Cactus
        cactuses = new GameObject[cactusCount];
        for (int i = 0; i < cactuses.Length; i++)
        {
            cactuses[i] = Instantiate(cactus, new Vector2(0, 0), Quaternion.identity);
            cactuses[i].SetActive(true);
            Rigidbody2D cactusRb = cactuses[i].GetComponent<Rigidbody2D>();
            cactusRb.velocity = new Vector2(-speed, 0);
        }

        //Static Objects
        treeRb = tree.GetComponent<Rigidbody2D>();
        treeRb.velocity = new Vector2(-speed, 0);

        block1Rb = block1.GetComponent<Rigidbody2D>();
        block2Rb = block2.GetComponent<Rigidbody2D>();
        block3Rb = block3.GetComponent<Rigidbody2D>();
        block1Rb.velocity = new Vector2(-speed, 0);
        block2Rb.velocity = new Vector2(-speed, 0);
        block3Rb.velocity = new Vector2(-speed, 0);

        cactus1Rb = cactus1.GetComponent<Rigidbody2D>();
        cactus2Rb = cactus2.GetComponent<Rigidbody2D>();
        cactus3Rb = cactus3.GetComponent<Rigidbody2D>();
        cactus4Rb = cactus4.GetComponent<Rigidbody2D>();
        cactus5Rb = cactus5.GetComponent<Rigidbody2D>();
        cactus6Rb = cactus6.GetComponent<Rigidbody2D>();
        cactus1Rb.velocity = new Vector2(-speed, 0);
        cactus2Rb.velocity = new Vector2(-speed, 0);
        cactus3Rb.velocity = new Vector2(-speed, 0);
        cactus4Rb.velocity = new Vector2(-speed, 0);
        cactus5Rb.velocity = new Vector2(-speed, 0);
        cactus6Rb.velocity = new Vector2(-speed, 0);

        cloud1Rb = cloud1.GetComponent<Rigidbody2D>();
        cloud2Rb = cloud2.GetComponent<Rigidbody2D>();
        cloud3Rb = cloud3.GetComponent<Rigidbody2D>();
        cloud4Rb = cloud4.GetComponent<Rigidbody2D>();
        cloud5Rb = cloud5.GetComponent<Rigidbody2D>();
        cloud1Rb.velocity = new Vector2(-speed / 1.5f, 0);
        cloud2Rb.velocity = new Vector2(-speed / 1.5f, 0);
        cloud3Rb.velocity = new Vector2(-speed / 1.5f, 0);
        cloud4Rb.velocity = new Vector2(-speed / 1.5f, 0);
        cloud5Rb.velocity = new Vector2(-speed / 1.5f, 0);
    }

	void Update () {
        
        //Background
        if (background1Rb.transform.position.x <= -bgWidth)
        {
            background1Rb.transform.position += new Vector3(bgWidth * 2, 0);
        }
        if (background2Rb.transform.position.x <= -bgWidth)
        {
            background2Rb.transform.position += new Vector3(bgWidth * 2, 0);
        }

        //Block
        blockTiming += Time.deltaTime;
        if (blockTiming > blockRandomTiming)
        {
            blockRandomTiming = Random.Range(blockTimingRange[0], blockTimingRange[1]);
            blockTiming = 0;
            blockGenerator = blocks[blockIndex].GetComponent<BlockGenerator>();
            blockGenerator.RandomBlock();

            blocks[blockIndex].transform.position = new Vector3(20, -4.07f);
            blockIndex++;

            if (blockIndex >= blocks.Length)
                blockIndex = 0;
        }

        //Cloud
        cloudTiming += Time.deltaTime;
        cloudRandomY = Random.Range(cloudRandomRangeY[0], cloudRandomRangeY[1]);
        if (cloudTiming > cloudRandomTiming)
        {
            cloudRandomTiming = Random.Range(cloudTimingRange[0], cloudTimingRange[1]);
            cloudTiming = 0;
            clouds[cloudIndex].transform.position = new Vector3(20f, cloudRandomY);
            cloudIndex++;

            if (cloudIndex >= clouds.Length)
                cloudIndex = 0;
        }

        //Cactus
        cactusTiming += Time.deltaTime;
        if (cactusTiming > cactusRandomTiming)
        {
            cactusRandomTiming = Random.Range(cactusTimingRange[0], cactusTimingRange[1]);
            cactusTiming = 0;
            cactuses[cactusIndex].transform.position = new Vector3(20, -4.66f);
            cactusIndex++;

            if (cactusIndex >= cactuses.Length)
                cactusIndex = 0;
        }

        //Static Objects
        destroyTiming += Time.deltaTime;
        if (destroyTiming > 20f)
        {
            tree.SetActive(false);
            block1.SetActive(false);
            block2.SetActive(false);
            block3.SetActive(false);
        }

        stopTiming += Time.deltaTime;
        if (stopScene && speed >= 0 && stopTiming > 0.3f)
        {
            speed = speed < 0f ? 0f : speed - 0.1f;
            stopTiming = 0;

            //Background
            background1Rb.velocity = new Vector2(-speed, 0);
            background2Rb.velocity = new Vector2(-speed, 0);

            //Block
            for (int i = 0; i < blocks.Length; i++)
            {
                Rigidbody2D blockRb = blocks[i].GetComponent<Rigidbody2D>();
                blockRb.velocity = new Vector2(-speed, 0);
            }

            //Cloud
            for (int i = 0; i < clouds.Length; i++)
            {
                Rigidbody2D cloudRb = clouds[i].GetComponent<Rigidbody2D>();
                cloudRb.velocity = new Vector2(-speed / 1.5f, 0);
            }

            //Cactus
            for (int i = 0; i < cactuses.Length; i++)
            {
                Rigidbody2D cactusRb = cactuses[i].GetComponent<Rigidbody2D>();
                cactusRb.velocity = new Vector2(-speed, 0);
            }

            //Static Objects
            treeRb.velocity = new Vector2(-speed, 0);

            block1Rb.velocity = new Vector3(-speed, 0);
            block2Rb.velocity = new Vector3(-speed, 0);
            block3Rb.velocity = new Vector3(-speed, 0);

            cactus1Rb.velocity = new Vector2(-speed, 0);
            cactus2Rb.velocity = new Vector2(-speed, 0);
            cactus3Rb.velocity = new Vector2(-speed, 0);
            cactus4Rb.velocity = new Vector2(-speed, 0);
            cactus5Rb.velocity = new Vector2(-speed, 0);
            cactus6Rb.velocity = new Vector2(-speed, 0);

            cloud1Rb.velocity = new Vector2(-speed, 0);
            cloud2Rb.velocity = new Vector2(-speed, 0);
            cloud3Rb.velocity = new Vector2(-speed, 0);
            cloud4Rb.velocity = new Vector2(-speed, 0);
            cloud5Rb.velocity = new Vector2(-speed, 0);
        }
    }

    public void StopScene()
    {
        stopScene = true;
    }
}
