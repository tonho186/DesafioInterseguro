package com.challenge.matrix;

import com.challenge.matrix.dto.MatrixPairRequest;
import com.challenge.matrix.dto.StatisticsResponse;
import com.challenge.matrix.service.MatrixStatisticsService;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.*;

class MatrixStatisticsServiceTest {

    private final MatrixStatisticsService service =
            new MatrixStatisticsService();

    @Test
    void shouldCalculateStatistics() {

        var request = new MatrixPairRequest(
                new double[][] {
                        {1, 2},
                        {3, 4}
                },
                new double[][] {
                        {5, 6},
                        {7, 8}
                }
        );

        StatisticsResponse result =
                service.calculate(request);

        assertEquals(8, result.max());
        assertEquals(1, result.min());
        assertEquals(4.5, result.average());
        assertEquals(36, result.sum());
        assertFalse(result.diagonal());
    }

    @Test
    void shouldDetectDiagonalMatrix() {

        var request = new MatrixPairRequest(
                new double[][] {
                        {1, 0},
                        {0, 2}
                },
                new double[][] {
                        {1, 2},
                        {3, 4}
                }
        );

        StatisticsResponse result =
                service.calculate(request);

        assertTrue(result.diagonal());
    }
}
