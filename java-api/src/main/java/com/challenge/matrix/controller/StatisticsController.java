package com.challenge.matrix.controller;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.challenge.matrix.dto.MatrixPairRequest;
import com.challenge.matrix.dto.StatisticsResponse;
import com.challenge.matrix.service.MatrixStatisticsService;

@RestController
@RequestMapping("/api/v1/statistics")
public class StatisticsController {

    private final MatrixStatisticsService service;

    public StatisticsController(
            MatrixStatisticsService service) {

        this.service = service;
    }

    @PostMapping
    public ResponseEntity<StatisticsResponse> calculate(
            @RequestBody MatrixPairRequest request) {

        return ResponseEntity.ok(
                service.calculate(request));
    }

    @GetMapping("/health")
    public ResponseEntity<String> health() {
        return ResponseEntity.ok("UP");
    }
}
