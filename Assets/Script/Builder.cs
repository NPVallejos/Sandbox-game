﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Main Purpose:
// - Handles everything that involves building, mining, attacking, and inventory management
public class Builder : MonoBehaviour
{
    private Vector2 mousePosition;
    private Vector3 buildDimensions;
    private Vector3 point;
    private Camera cam;
    private BoxCollider2D buildBoxCol;
    public int buildLayerMask;

    public GameObject player;

    private Vector3 playerPos;
    private bool canBuild;
    private bool canPlace;
    private bool canMine;

    public GameObject templateObject; // TODO: Grab template object from prefab folder
    private Vector3 templateObjPos;
    private BoxCollider2D templateObjCol;
    private SpriteRenderer sprRend;

    public Inventory inventory;
    public IEnumerator mineCoroutine;
    private GameObject currentSingleton;

    void Awake() {
        cam = Camera.main;
        buildBoxCol = GetComponent<BoxCollider2D>();
        templateObjCol = templateObject.GetComponent<BoxCollider2D>();
        sprRend = templateObject.GetComponent<SpriteRenderer>();

        if (sprRend != null)
            sprRend.color = new Color(1f, 1f, 1f, 0.5f);

        if (buildBoxCol != null)
            buildDimensions = new Vector3(buildBoxCol.bounds.size.x * 0.5f, buildBoxCol.bounds.size.y * 0.5f, 0f);
    }

    void Start() {
        canBuild = true;
        canPlace = false;
        mineCoroutine = null;
        currentSingleton = null;
        inventory.reset();
    }

    public void Update() // TODO: Make a dynamic object pooling system
    {
        mousePosition = Input.mousePosition;
        playerPos = player.transform.position;
        displayTemplateObject();

        if(Input.GetAxis("Mouse ScrollWheel") > 0) {
            inventory.cycleUp();
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0) {
            inventory.cycleDown();
        }
        if (Input.GetButtonDown("Fire2")) {
            if(mineCoroutine != null) {
                StopCoroutine(mineCoroutine);
                Debug.Log("Stopped & Replaced Coroutine");
            }

            mineCoroutine = mineObject();
            StartCoroutine(mineCoroutine);
            Debug.Log("Started Coroutine");
        }
    }

    public void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
            placeObject();
        // if(Input.GetButton("Fire2"))
        //     checkRays();
    }

    public void displayTemplateObject()
    {
        if (templateObject != null)
        {
            templateObjPos = templateObject.transform.position;
            point = cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, cam.nearClipPlane));
            point -= Camera.main.transform.position;
            templateObjPos = Camera.main.transform.position + point;
            if(!inBuildBoundary()) {
                templateObject.transform.position = new Vector3(0, 0, cam.transform.position.z);  // Hides the template object
                canPlace = false;
                canMine = false;
            }
            else {
                templateObject.transform.position = new Vector3((int)templateObjPos.x, (int)templateObjPos.y, 0f); // Otherwise we can display the template object
                canMine = true;
            }
        }
    }

    public bool inBuildBoundary() {
        // Checking if template object is within build boundary
        if (vecL(templateObjPos, (playerPos - buildDimensions)) || vecG(templateObjPos, (playerPos + buildDimensions)))
        {
            return false;
        }
        // Checking if template object is within player BoxCollider2D
        // If true then we do not want to display the template object over the player, i.e. not a valid spot to build
        templateObjPos += roundPos(templateObjPos);
        if (new Vector3((int)templateObjPos.x, (int)templateObjPos.y) == playerPos)
        {
            return false;
        }
        return true;
    }

    public void placeObject()
    {
        checkRays();
        if (canBuild && canPlace && inventory.isBlock()) //&& inv.getTotal() > 0 && inv.placeableObj())
        {
            sprRend.color = new Color(1f, 1f, 1f, 1f);
            //currentObject.layer = currentObjMask;

            //Object.Instantiate(inventory[currentItem], templateObject.transform.position, template.transform.rotation);
            //inventoryCount[0].text = inv.toString();

            sprRend.color = new Color(1f, 1f, 1f, 0.5f);
            //currentObject.layer = 0;
        }
    }

    public IEnumerator mineObject()
    {
        do {
            if (checkDestroyRay() && canMine && inventory.checkItemFunction(0)) // && inv.getObjName() == "picaxe_wood")
            {
                // See Singleton.cs for more details on dealDamage()
                currentSingleton.GetComponent<Singleton>().dealDamage(inventory.getToolPower());
                yield return new WaitForSeconds(0.2f);
                yield return null;
            }
        } while (Input.GetButton("Fire2"));
        yield return new WaitForSeconds(0.2f);
    }

    public bool checkDestroyRay() {
        float raySize = 0.1f;
        templateObjPos = templateObject.transform.position;

        Vector2 origin = new Vector2(templateObjCol.bounds.center.x, templateObjCol.bounds.center.y);
        RaycastHit2D outRay = Physics2D.Raycast(origin, Vector2.up, raySize, 1 << buildLayerMask);

        Debug.DrawRay(origin, Vector2.up * raySize, Color.yellow);

        if(outRay.collider != null) {
            currentSingleton = outRay.collider.gameObject;
            return true;
        }
        return false;
    }

    public void checkRays()
    {
        templateObjPos = templateObject.transform.position;
        RaycastHit2D[] outRays = new RaycastHit2D[10];
        Vector2[] origin = new Vector2[9];
        float raySize = 0.1f;
        float skinWidth = 0.015f;

        // Store origin at the midpoints of each side of the box collider
        origin[0] = new Vector2(templateObjCol.bounds.center.x, templateObjCol.bounds.min.y - skinWidth);
        origin[1] = new Vector2(templateObjCol.bounds.center.x, templateObjCol.bounds.max.y + skinWidth);
        origin[2] = new Vector2(templateObjCol.bounds.min.x - skinWidth, templateObjCol.bounds.center.y);
        origin[3] = new Vector2(templateObjCol.bounds.max.x + skinWidth, templateObjCol.bounds.center.y);
        // This is a ray used to detect if a block is already placed at target location
        origin[4] = new Vector2(templateObjCol.bounds.center.x, templateObjCol.bounds.center.y);
        // The next 4 rays will detect the player
        origin[5] = new Vector2(templateObjCol.bounds.min.x + skinWidth, templateObjCol.bounds.min.y + skinWidth);
        origin[6] = new Vector2(templateObjCol.bounds.min.x + skinWidth, templateObjCol.bounds.max.y - skinWidth);
        origin[7] = new Vector2(templateObjCol.bounds.max.x - skinWidth, templateObjCol.bounds.max.y - skinWidth);
        origin[8] = new Vector2(templateObjCol.bounds.max.x - skinWidth, templateObjCol.bounds.min.y + skinWidth);

        // Cast a ray straight down, up, left, right
        outRays[0] = Physics2D.Raycast(origin[0], -Vector2.up, raySize, 1 << buildLayerMask);
        outRays[1] = Physics2D.Raycast(origin[1], Vector2.up, raySize, 1 << buildLayerMask);
        outRays[2] = Physics2D.Raycast(origin[2], -Vector2.right, raySize, 1 << buildLayerMask);
        outRays[3] = Physics2D.Raycast(origin[3], Vector2.right, raySize, 1 << buildLayerMask);
        // This is a ray used to detect if a block is already placed at target location
        outRays[4] = Physics2D.Raycast(origin[4], Vector2.up, raySize, 1 << buildLayerMask);
        // The next 5 rays will detect the player
        outRays[5] = Physics2D.Raycast(origin[5], Vector2.up, 0.9f, 1 << player.layer);
        outRays[6] = Physics2D.Raycast(origin[6], Vector2.right, 0.9f, 1 << player.layer);
        outRays[7] = Physics2D.Raycast(origin[7], -Vector2.up, 0.9f, 1 << player.layer);
        outRays[8] = Physics2D.Raycast(origin[8], -Vector2.right, 0.9f, 1 << player.layer);
        outRays[9] = Physics2D.Raycast(origin[5], new Vector2(0.9f, 0.9f), 1f, 1 << player.layer);


        // Draw the rays w.t.r. to individual origin and direction
        Debug.DrawRay(origin[0], -Vector2.up * raySize, Color.white);
        Debug.DrawRay(origin[1], Vector2.up * raySize, Color.white);
        Debug.DrawRay(origin[2], -Vector2.right * raySize, Color.white);
        Debug.DrawRay(origin[3], Vector2.right * raySize, Color.white);
        Debug.DrawRay(origin[4], Vector2.up * raySize, Color.yellow);
        Debug.DrawRay(origin[5], Vector2.up * 0.9f, Color.blue);
        Debug.DrawRay(origin[6], Vector2.right * 0.9f, Color.blue);
        Debug.DrawRay(origin[7], -Vector2.up * 0.9f, Color.blue);
        Debug.DrawRay(origin[8], -Vector2.right * 0.9f, Color.blue);
        Debug.DrawRay(origin[5], new Vector2(0.9f, 0.9f), Color.blue);

        canPlace = false;

        for (int i = 0; i < outRays.Length; i++) {
            // If it hits something...
            if (outRays[i].collider != null) {
                if (i < 4)
                    canPlace = true;
                else if (i == 4)
                    canMine = true;
                else
                    canPlace = false;
            }
            // If it misses...
            else {
                // And its the ray that detects another block...
                if(i == 4)
                    canMine = false;
            }
        }
    }


    public Vector3 roundPos(Vector3 pos) {
        float xRound = 0.5f;
        float yRound = 0.5f;
        if (pos.x < 0)
        {
            xRound = -0.5f;
        }
        if (pos.y < 0)
        {
            yRound = -0.5f;
        }
        return new Vector3(xRound, yRound, 0.0f);
    }

    public bool vecL(Vector3 a, Vector3 b) {
        if (a.x < b.x || a.y < b.y)
            return true;
        return false;
    }

    public bool vecG(Vector3 a, Vector3 b) {
        if (a.x > b.x || a.y > b.y)
            return true;
        return false;
    }
}
