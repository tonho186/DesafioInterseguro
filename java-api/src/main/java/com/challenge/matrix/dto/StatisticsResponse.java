package com.challenge.matrix.dto;

public record StatisticsResponse(
        double max,
        double min,
        double average,
        double sum,
        boolean diagonal) {
}
