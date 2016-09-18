using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move {
    public Position start;
    public Position end;

    public List<Piece> eatenPieces = new List<Piece>();
    public bool pieceEvolved = false;

    public bool isAttack = false;

    public Move (Position start, Position end) {
        this.start = start;
        this.end = end;
    }

    public Move (Position start, Position end, bool isAttack) {
        this.start = start;
        this.end = end;
        this.isAttack = isAttack;
    }

    public override string ToString () {
        return "start: " + start + "; end " + end;
    }

    public static bool operator == (Move move1, Move move2) {
        if ((System.Object)move1 == null || (System.Object)move2 == null) {
            return false;
        }

        return move1.start == move2.start && move1.end == move2.end;
    }

    public static bool operator != (Move move1, Move move2) {
        return !(move1 == move2);
    }

    public bool Equals (Move other) {
        return this == other;
    }

    public override bool Equals (object obj) {
        if (obj == null) {
            return false;
        }

        Move objAsMove = obj as Move;
        if (objAsMove == null) {
            return false;
        } else {
            return Equals(objAsMove);
        }
    }

    public override int GetHashCode () {
        return base.GetHashCode();
    }
}