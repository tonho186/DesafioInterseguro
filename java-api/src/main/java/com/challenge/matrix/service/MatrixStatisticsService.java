package com.challenge.matrix.service;

import org.springframework.stereotype.Service;

import com.challenge.matrix.dto.MatrixPairRequest;
import com.challenge.matrix.dto.StatisticsResponse;

@Service
public class MatrixStatisticsService {

    private static final double EPSILON = 1e-10;

    public StatisticsResponse calculate(MatrixPairRequest request) {

        validate(request);

        StatisticsAccumulator accumulator =
                new StatisticsAccumulator();

        accumulate(request.q(), accumulator);
        accumulate(request.r(), accumulator);

        boolean diagonal =
                isDiagonal(request.q()) ||
                isDiagonal(request.r());

        return new StatisticsResponse(
                accumulator.max,
                accumulator.min,
                accumulator.sum / accumulator.count,
                accumulator.sum,
                diagonal
        );
    }

    private void accumulate(
            double[][] matrix,
            StatisticsAccumulator accumulator) {

        for (double[] row : matrix) {
            for (double value : row) {

                if (Double.isNaN(value) ||
                    Double.isInfinite(value)) {

                    throw new IllegalArgumentException(
                            "Matrix values must be finite.");
                }

                accumulator.max =
                        Math.max(accumulator.max, value);

                accumulator.min =
                        Math.min(accumulator.min, value);

                accumulator.sum += value;
                accumulator.count++;
            }
        }
    }

    private boolean isDiagonal(double[][] matrix) {

        if (matrix.length == 0 ||
            matrix.length != matrix[0].length) {

            return false;
        }

        for (int i = 0; i < matrix.length; i++) {

            if (matrix[i].length != matrix.length) {
                return false;
            }

            for (int j = 0; j < matrix[i].length; j++) {

                if (i != j &&
                    Math.abs(matrix[i][j]) > EPSILON) {

                    return false;
                }
            }
        }

        return true;
    }

    private void validate(MatrixPairRequest request) {

        if (request == null) {
            throw new IllegalArgumentException(
                    "Request cannot be null.");
        }

        validateMatrix(request.q(), "Q");
        validateMatrix(request.r(), "R");
    }

    private void validateMatrix(
            double[][] matrix,
            String name) {

        if (matrix == null ||
            matrix.length == 0) {

            throw new IllegalArgumentException(
                    name + " cannot be empty.");
        }

        if (matrix[0] == null ||
            matrix[0].length == 0) {

            throw new IllegalArgumentException(
                    name + " must contain columns.");
        }

        int columns = matrix[0].length;

        for (double[] row : matrix) {

            if (row == null ||
                row.length != columns) {

                throw new IllegalArgumentException(
                        name + " must be rectangular.");
            }
        }
    }

    private static class StatisticsAccumulator {

        private double max = -Double.MAX_VALUE;
        private double min = Double.MAX_VALUE;
        private double sum = 0;
        private long count = 0;
    }
}
