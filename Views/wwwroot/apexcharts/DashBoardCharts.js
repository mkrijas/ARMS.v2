
window.ChartResult = {
    PieChart: function (labels, data) {
        var options = {
            series: data,
            labels: labels,
            chart: {
                width: '100%',
                height: '100%',
                type: 'pie',
            },
            dataLabels: {
                enabled: true,
                formatter: function (val, opts) {
                    // Accessing the corresponding value from the 'data' array
                    return opts.w.globals.series[opts.seriesIndex]; // Assuming 'opts' contains the index
                }
            },
            responsive: [{
                breakpoint: 200,
                options: {
                    chart: {
                        minWidth: 300
                    },
                    legend: {
                        position: 'center'
                    }
                }
            }]
        };

        var chart = new ApexCharts(document.querySelector("#PieChart"), options);
        chart.render();
    },

    BarGraph: function (labels, data) {
        var options = {
            series: [{
                name: 'Total Quantity',
                data: data
            }],
            chart: {
                width: '100%',
                height: '100%',
                type: 'bar',
            },
            margin: {
                top: 20,
                right: 20,
                bottom: 20,
                left: 20,
            },
            responsive: [{
                breakpoint: 510,
                options: {
                    chart: {
                        width: 510,
                    },
                    legend: {
                        position: 'bottom'
                    }
                }
            }],
            plotOptions: {
                bar: {
                    borderRadius: 2,
                    dataLabels: {
                        position: 'top', // top, center, bottom
                    },
                }
            },
            dataLabels: {
                enabled: true,
                formatter: function (val) {
                    return val === 0.00 ? "" : val;
                },
                offsetY: -20,
                style: {
                    fontSize: '12px',
                    colors: ["#304758"]
                }
            },
            xaxis: {
                categories: labels,
                position: 'bottom',
                axisBorder: {
                    show: false
                },
                axisTicks: {
                    show: false
                },
                crosshairs: {
                    fill: {
                        type: 'gradient',
                        gradient: {
                            colorFrom: '#D8E3F0',
                            colorTo: '#BED1E6',
                            stops: [0, 100],
                            opacityFrom: 0.4,
                            opacityTo: 0.5,
                        }
                    }
                },
                tooltip: {
                    enabled: true,
                }
            },
            yaxis: {
                axisBorder: {
                    show: false
                },
                axisTicks: {
                    show: false,
                },
                labels: {
                    show: true,
                    formatter: function (val) {
                        return val;
                    }
                }
            }
        };
        var chart = new ApexCharts(document.querySelector("#BarGraph"), options);
        if (chart) {
            chart.render();
        }
        if (chart && chart.updateOptions) {
            chart.updateOptions({
                series: [{
                    data: data
                }],
                xaxis: {
                    categories: labels
                }
            });
        }
    },

    DonutChart: function (labels, data) {
        var options = {
            series: data,
            labels: labels,
            chart: {
                width: '100%',
                height: '100%',
                type: 'donut',
            },
            dataLabels: {
                enabled: true,
                formatter: function (val, opts) {
                    // Accessing the corresponding value from the 'data' array
                    return opts.w.globals.series[opts.seriesIndex]; // Assuming 'opts' contains the index
                }
            },
            plotOptions: {
                pie: {
                    donut: {
                        labels: {
                            show: true,
                            total: {
                                showAlways: true,
                                show: true,
                                label: 'Total Drivers',
                            }
                        }
                    }
                }
            },
            responsive: [{
                breakpoint: 200,
                options: {
                    chart: {
                        minWidth: 300
                    },
                    legend: {
                        position: 'bottom'
                    }
                }
            }]
        }

        var chart = new ApexCharts(document.querySelector("#DonutChart"), options);
        chart.render();
    },

    LineGraph: function (labels, data, Cumulativedata) {
        var options = {
            series: [
                {
                    name: "Cumulative Target Data",
                    data: Cumulativedata,
                },
                {
                    name: "Cumulative Freight Per Day",
                    data: data
                }
            ],
            chart: {
                width: '100%',
                height: 500,
                type: 'line',
                dropShadow: {
                    enabled: true,
                    color: '#000',
                    top: 18,
                    left: 7,
                    blur: 10,
                    opacity: 0.2
                },
                toolbar: {
                    show: true,
                    fontSize: 20,
                    autoSelected: 'zoom',
                },
                zoom: {
                    type: 'x',
                    enabled: true,
                    autoScaleYaxis: true
                },
            },
            colors: ['#77B6EA', '#545454'],
            dataLabels: {
                enabled: true,
            },
            stroke: {
                curve: 'smooth'
            },
            grid: {
                borderColor: '#e7e7e7',
                row: {
                    colors: ['#f3f3f3', 'transparent'], // takes an array which will be repeated on columns
                    opacity: 0
                },
            },
            markers: {
                size: 1
            },
            xaxis: {
                categories: labels,
                title: {
                    text: 'DAYS OF THE MONTH'
                }
            },
            yaxis: {
                title: {
                    text: 'CUMULATIVE FREIGHT AMOUNT'
                },
                //min: 5,
                //max: 40
            },
            legend: {
                position: 'top',
                horizontalAlign: 'right',
                floating: true,
                offsetY: -25,
                offsetX: -5
            }
        };

        var chart = new ApexCharts(document.querySelector("#LineGraph"), options);
        chart.render();
    }
};