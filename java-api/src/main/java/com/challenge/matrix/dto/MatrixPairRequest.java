package com.challenge.matrix.dto;

public record MatrixPairRequest(
        double[][] q,
        double[][] r) {
}