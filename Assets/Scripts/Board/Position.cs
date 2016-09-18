using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Position {
    public int x;
    public int y;
    public Field field;

    public Position (int x, int y, Field field) {
        this.x = x;
        this.y = y;
        this.field = field;
    }

    public override string ToString () {
        return "(" + x + ", " + y + ")";
    }

    public static bool operator == (Position position1, Position position2) {
        if ((System.Object)position1 == null || (System.Object)position2 == null) {
            return false;
        }

        return position1.x == position2.x && position1.y == position2.y;
    }

    public static Position operator + (Position position1, Position position2) {
        if ((System.Object)position1 == null) {
            return position2;
        }
        if ((System.Object)position2 == null) {
            return position1;
        }

        Board board = GameObject.FindGameObjectWithTag("board").GetComponent<Board>();
        int x = position1.x + position2.x;
        int y = position1.y + position2.y;

        return new Position(x, y, board.ValidPosition(x, y) ? board.GetField(x, y) : null);
    }

    public static Position operator * (int k, Position position) {
        if ((System.Object)position == null) {
            return null;
        }

        return new Position(k * position.x, k * position.y, position.field);
    }

    public static Position operator * (Position position, int k) {
        return k * position;
    }

    public static bool operator != (Position position1, Position position2) {
        return !(position1 == position2);
    }

    public bool Equals (Position other) {
        return this == other;
    }

    public override bool Equals (object obj) {
        if (obj == null) {
            return false;
        }

        Position objAsPosition = obj as Position;
        if (objAsPosition == null) {
            return false;
        } else {
            return Equals(objAsPosition);
        }
    }

    public override int GetHashCode () {
        return base.GetHashCode();
    }
}

public class Direction : Position {
    public Direction (int x, int y, Field field) : base(x, y, field) { }
}