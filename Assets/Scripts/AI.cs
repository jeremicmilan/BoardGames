using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AI {
    public Game game;
    public Board board;

    public const int MAX_DEPTH = 2;

    public AI (Game game) {
        this.game = game;
    }

    public Move minimax () {
        return max().Key;
    }

    private KeyValuePair<Move, int> min (int depth = 0, int currentMax = int.MinValue, Move previousMove = null) {
        if (depth >= MAX_DEPTH) {
            return new KeyValuePair<Move, int>(previousMove, game.scoreBoard(game.isWhitesTurn));
        }

        Move bestMove = null;
        int min = int.MaxValue;

        foreach (Move move in board.GetAllMoves(game.isWhitesTurn)) {
            if (currentMax >= min) {
                continue;
            }

            game.MakeMove(move, fake: true);

            int tmpMin = max(depth + 1, min, move).Value;
            if (tmpMin < min) {
                min = tmpMin;
                bestMove = move;
            }

            board.Undo(fake: true);
        }

        return new KeyValuePair<Move, int>(bestMove, min);
    }

    private KeyValuePair<Move, int> max (int depth = 0, int currentMin = int.MaxValue, Move previousMove = null) {
        if (depth >= MAX_DEPTH) {
            return new KeyValuePair<Move, int>(previousMove, game.scoreBoard(game.isWhitesTurn));
        }

        Move bestMove = null;
        int max = int.MinValue;

        foreach (Move move in board.GetAllMoves(game.isWhitesTurn)) {
            if (currentMin <= max) {
                continue;
            }

            game.MakeMove(move, fake: true);

            int tmpMax = min(depth + 1, max, move).Value;
            if (tmpMax > max) {
                max = tmpMax;
                bestMove = move;
            }

            board.Undo(fake: true);
        }

        return new KeyValuePair<Move, int>(bestMove, max);
    }
}
