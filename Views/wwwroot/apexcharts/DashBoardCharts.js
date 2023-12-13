
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
                top: 20, // Adjust the top margin
                right: 20, // Adjust the right margin
                bottom: 20, // Adjust the bottom margin
                left: 20 // Adjust the left margin
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
    }
};