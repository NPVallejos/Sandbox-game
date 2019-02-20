using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private bool canDestroy;

    public GameObject templateObject; // TODO: Grab template object from prefab folder
    private Vector3 templateObjPos;
    private BoxCollider2D templateObjCol;
    private RaycastHit2D[] outRays;
    private SpriteRenderer sprRend;

    public Inventory inventory;
    public short currentItem;
    private Weapon wp = new Weapon("axe", 2, null, 0, 1, 3, 3, "fire");

    void Awake()
    {
        currentItem = 0;
        inventory.Add(wp);
        inventory.Add(wp);
        inventory.Get(currentItem).attack();
        inventory.Get(++currentItem).attack();

        canBuild = true;
        canPlace = false;

        cam = Camera.main;
        buildBoxCol = GetComponent<BoxCollider2D>();
        templateObjCol = templateObject.GetComponent<BoxCollider2D>();
        sprRend = templateObject.GetComponent<SpriteRenderer>();

        if (sprRend != null)
            sprRend.color = new Color(1f, 1f, 1f, 0.5f);

        if (buildBoxCol != null)
            buildDimensions = new Vector3(buildBoxCol.bounds.size.x * 0.5f, buildBoxCol.bounds.size.y * 0.5f, 0f);
    }

    public void Update() // TODO: Make a dynamic object pooling system
    {
        mousePosition = Input.mousePosition;
        playerPos = player.transform.position;
        displayTemplateObject();
    }

    public void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
            placeObject();
        if (Input.GetButton("Fire2"))
            destroyObject();
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
                canDestroy = false;
            }
            else {
                templateObject.transform.position = new Vector3((int)templateObjPos.x, (int)templateObjPos.y, 0f); // Otherwise we can display the template object
                canDestroy = true; // Now we cannot set canDestroy to true just yet; we need to know what item the player is currently holding
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

    // So now we need an inventory system where we keep track of the current inventory index
    // As well as the current shortcut items
    // So we need an inventory filled with prefabs whose layer is 8
    public void placeObject()
    {
        checkRays();
        //Debug.Log(canBuild + " : " + canPlace);
        if (canBuild && canPlace) //&& inv.getTotal() > 0 && inv.placeableObj())
        {
            sprRend.color = new Color(1f, 1f, 1f, 1f);
            //currentObject.layer = currentObjMask;

            //Object.Instantiate(inventory[currentItem], templateObject.transform.position, template.transform.rotation);
            //inventoryCount[0].text = inv.toString();

            sprRend.color = new Color(1f, 1f, 1f, 0.5f);
            //currentObject.layer = 0;
        }
    }

    public void destroyObject()
    {
        checkRays();
        if (canBuild && canDestroy) // && inv.getObjName() == "picaxe_wood")
        {
            // TODO: destroy the object that has this collider
            Object.Destroy(outRays[4].collider.gameObject);
            //inv.incrementItemCount(outRays[4].collider.gameObject.name);
            //inventoryCount[0].text = inv.toString();
        }
    }

    public void checkRays()
    {
        templateObjPos = templateObject.transform.position;
        outRays = new RaycastHit2D[10];
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
                    canDestroy = true;
                else
                    canPlace = false;
            }
            // If it misses...
            else {
                // And its the ray that detects another block...
                if(i == 4)
                    canDestroy = false;
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
