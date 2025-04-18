using UnityEngine;

public class Region {
    private Vector2 start;
    private Vector2 end;

    private RegionType type;

    public Region( Vector2 start, Vector2 end, RegionType type ) {
        this.start = start;
        this.end = end;
        this.type = type;
    }
}