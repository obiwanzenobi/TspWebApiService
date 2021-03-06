﻿var baseGraph;
var pathGraphs = [];

var minNumberOfNodes = 1;
var maxNumberOfNodes = 15;
var pathGraphsEnableEdgeHovering = false;
var baseGraphEnableEdgeHovering = true;
var maxIntegerInputLength = 3;
var maxFloatInputLength = 4;


$(document).ready(function () {
    window.blockUIImage = {
        message: $("#loading-gif"),
        baseZ: 2000,
        css: { "background-color": "transparent", "border": "none" },
        overlayCSS: { backgroundColor: 'white', opacity: 0.4 }
    };

    $('.float').keypress(function (event) {
        if (((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) || $(this).val().length >= maxFloatInputLength) {
            event.preventDefault();
        }
    });

    $('.integer').keypress(function (event) {
        if ((event.which < 48 || event.which > 57) || $(this).val().length >= maxIntegerInputLength) {
            event.preventDefault();
        }
    });

    $("#computePathBtn").attr("disabled", true);

    $("#drawGraphBtn").click(function () {
        var n = $("#nValue").val();

        if (n >= minNumberOfNodes && n <= maxNumberOfNodes) {
            if (baseGraph) {
                baseGraph.graph.clear();
                baseGraph.refresh();
            }

            baseGraph = new sigma({
                graph: drawBaseGraph(parseInt(n)),
                renderer: {
                    container: document.getElementById('base-graph-container'),
                    type: 'canvas'
                },
                settings: {
                    enableEdgeHovering: baseGraphEnableEdgeHovering,
                    edgeHoverSizeRatio: 2,
                    edgeLabelSize: 'proportional',
                    defaultEdgeLabelSize: 14,
                    defaultEdgeLabelColor: 'green',
                    zoomingRatio: 1.0,
                    minEdgeSize: 2,
                    maxEdgeSize: 2
                }
            });
            sigma.plugins.dragNodes(baseGraph, baseGraph.renderers[0]);

            $("#computePathBtn").attr("disabled", false);
        }
        else {
            showModalWarning("Należy podać poprawną liczbę wierzchołków grafu z przedziału (" + minNumberOfNodes + ";" + maxNumberOfNodes + ")");
        }
    });

    $("#computePathBtn").click(function () {
        var popSize = $("#pValue").val();
        var mutationRate = $("#mValue").val();
        var crossoverRate = $("#kValue").val();
        var iterationsNumber = $("#iValue").val();

        if (popSize.length > 0 && mutationRate.length > 0 && crossoverRate.length > 0 && iterationsNumber.length > 0) {
           // $.blockUI(window.blockUIImage);

            var model =
                {
                    PopSize: parseInt(popSize),
                    MutationRate: parseFloat(mutationRate),
                    CrossoverRate: parseFloat(crossoverRate),
                    IterationsNumber: parseInt(iterationsNumber),
                    IdList: prepareMatrixOfNodesIds(),
                    WeightMatrix: prepareMatrixOfEdges()
                };

            $.ajax({
                type: "POST",
                contentType: "application/json",
                dataType: 'json',
                url: '/api/tspresolve',
                data: JSON.stringify(model),
                success: function (result) {

                    google.charts.load('current', { 'packages': ['corechart'] });
                    google.charts.setOnLoadCallback(drawChart);

                    function drawChart() {
                        drawLineChart(result.Iterations);
                        drawScatterChart(result.Iterations);
                    };

                    for (var i = 0; i < pathGraphs.length; i++) {
                        pathGraphs[i].graph.clear();
                        pathGraphs[i].refresh();
                    }
                    pathGraphs = [];

                    var theBestPath = result.Iterations[result.Iterations.length - 1].Resolves[0].OrderedIdList;
                    var permutates = permutator(theBestPath);

                    drawPathGraph("first-path-graph-container", permutates[Math.ceil(Math.random() * permutates.length)]);
                    drawPathGraph("second-path-graph-container", permutates[Math.ceil(Math.random() * permutates.length)]);
                    drawPathGraph("third-path-graph-container", permutates[Math.ceil(Math.random() * permutates.length)]);
                    drawPathGraph("fourth-path-graph-container", permutates[Math.ceil(Math.random() * permutates.length)]);

                  //  $.unblockUI();
                },
                error: function () {
                  // $.unblockUI();
                }
            });
        }
        else {
            showModalWarning("Należy podać wszystkie wymagane parametry");
        }
    });

    $('#clearBtn').click(function () {
        location.reload(true);
    });
});


function drawPathGraph(container, nodes) {
    var g = {
        nodes: [],
        edges: []
    };

    var baseNodes = baseGraph.graph.nodes();

    for (var i = 0; i < baseNodes.length; i++) {
        g.nodes.push({
            id: container + "_" + i,
            x: baseNodes[i].x,
            y: baseNodes[i].y,
            label: baseNodes[i].label,
            size: 1,
            color: '#617db4'
        });
    }

    for (var i = 0; i < nodes.length - 1; i++) {
        g.edges.push({
            id: container + "_" + "e" + "_" + i,
            source: container + "_" + nodes[i],
            target: container + "_" + nodes[i + 1],
            size: 1,
            color: '#ccc',
            type: ['line']
        });
    }

    pathGraphs.push(new sigma({
        graph: g,
        renderer: {
            container: document.getElementById(container),
            type: 'canvas'
        },
        settings: {
            enableEdgeHovering: pathGraphsEnableEdgeHovering,
            edgeHoverSizeRatio: 3,
            edgeLabelSize: 'proportional',
            defaultEdgeLabelSize: 13,
            defaultEdgeLabelColor: 'green',
            zoomingRatio: 1.0,
            minNodeSize: 1,
            maxNodeSize: 6,
            minEdgeSize: 2,
            maxEdgeSize: 2
        }
    }));
};

function drawBaseGraph(n) {
    var g = {
        nodes: [],
        edges: []
    };

    for (var i = 0; i < n; i++) {
        g.nodes.push({
            id: 'n' + i,
            index: i,
            label: 'N' + (i + 1),
            x: Math.random(),
            y: Math.random(),
            size: 1,
            color: '#617db4'
        });
    }

    for (i = n - 1; i >= 0; i--) {
        for (var j = 0; j < i; j++) {
            if (i != j) {
                var distance = Math.ceil(computeDistanceBetweenNodes(g.nodes[i].x, g.nodes[i].y, g.nodes[j].x, g.nodes[j].y) * 9);
                g.edges.push({
                    id: 'e' + "_" + i + "_" + j,
                    source: 'n' + i,
                    target: 'n' + j,
                    size: 1,
                    color: '#ccc',
                    label: "" + distance,
                    type: ['line']
                });
            }
        }
    }
    return g;
};

function drawLineChart(iterations) {
    var rawData = [];
    rawData.push(['X', 'F1', 'F2']);
    for (var i = 0; i < iterations.length; i++) {
        var item = [iterations[i].Id, iterations[i].Resolves[iterations[i].BestFirstFuncResolveIndex].FirstCost, iterations[i].Resolves[iterations[i].BestSecondFuncResolveIndex].SecondCost];
        rawData.push(item);
    }

    var data = google.visualization.arrayToDataTable(rawData);

    var options = {
        title: 'Wartośći funkcji wzlędem iteracji',
        legend: { position: 'right' },
        hAxis: { title: 'Iteracje' },
        vAxis: { title: 'Wartości funkcji' },
        height: 400
    };

    var chart = new google.visualization.LineChart(document.getElementById('chart_div_line'));
    chart.draw(data, options);
};

function drawScatterChart(iterations) {
    var rawData = [];

    for (var i = 0; i < iterations.length; i++) {
        var item = [iterations[i].Resolves[iterations[i].BestFirstFuncResolveIndex].FirstCost, iterations[i].Resolves[iterations[i].BestSecondFuncResolveIndex].SecondCost];
        item[2] = computeDistanceBetweenNodes(0, 0, item[0], item[1]);
        rawData.push(item);
    }

    var minValue = 999999;
    for (var i = 0; i < rawData.length; i++) {
        if (minValue > rawData[i][2])
        {
            minValue = rawData[i][2];
        }
    }

    var options = {
        title: 'Wartośći osobników populacji',
        hAxis: { title: 'F1', minValue: 0 },
        vAxis: { title: 'F2', minValue: 0 },
        legend: 'none',
        height: 400
    };

    var data = new google.visualization.DataTable();
    data.addColumn('number', '');
    data.addColumn('number', '');
    data.addColumn({ 'type': 'string', 'role': 'style' });

    for (var i = 0; i < rawData.length; i++) {
        if (rawData[i][2] == minValue) {
            data.addRow([rawData[i][0], rawData[i][1], 'point {fill-color: red']);
        }
        else {
            data.addRow([rawData[i][0], rawData[i][1], '']);
        }
    }

    var chart = new google.visualization.ScatterChart(document.getElementById('chart_div_scatter'));
    chart.draw(data, options);
};

function computeDistanceBetweenNodes(x1, y1, x2, y2) {
    return Math.sqrt(Math.pow(x1 - x2, 2) + Math.pow(y1 - y2, 2));
};

function prepareMatrixOfEdges() {
    var xNodes = baseGraph.graph.nodes();
    var xEdges = baseGraph.graph.edges();

    var matrixOfEdges = [];

    for (var i = 0; i < xNodes.length; i++) {
        var tempArray = [];
        for (var j = 0; j < xNodes.length; j++) {
            if (xNodes[i].id == xNodes[j].id) {
                tempArray[j] = 0;
            }
            else {
                for (var k = 0; k < xEdges.length; k++) {
                    if (xEdges[k].source == xNodes[i].id && xEdges[k].target == xNodes[j].id
                        || xEdges[k].target == xNodes[i].id && xEdges[k].source == xNodes[j].id) {
                        tempArray[j] = xEdges[k].label;
                    }
                }
            }
        }
        matrixOfEdges.push(tempArray);
    }
    return matrixOfEdges;
};

function prepareMatrixOfNodesIds() {
    var xNodes = baseGraph.graph.nodes();
    var indexList = [];
    for (var i = 0; i < xNodes.length; i++) {
        indexList.push(parseInt(xNodes[i].index));
    }
    return indexList;
};

function permutator(inputArr) {
    var results = [];

    function permute(arr, memo) {
        var cur, memo = memo || [];

        for (var i = 0; i < arr.length; i++) {
            cur = arr.splice(i, 1);
            if (arr.length === 0) {
                results.push(memo.concat(cur));
            }
            permute(arr.slice(), memo.concat(cur));
            arr.splice(i, 0, cur[0]);
        }
        return results;
    }
    return permute(inputArr);
};

function showModalWarning(message) {
    $("#warningMessage").text(message);
    $('#warningModal').modal('show');
};