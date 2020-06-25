function generateReport()
{
    var container = document.getElementById("canvasContainer");
    var canvas = document.createElement("canvas");
    getGraphic(canvas);
    container.appendChild(canvas);
}

function getGraphic(canvas)
{
    new Chart(canvas, {
        type: 'bar',
        data: {
            labels: ["Africa", "Asia", "Europe", "Latin America", "North America"],
            datasets: [
                {
                    label: "Population (millions)",
                    backgroundColor: ["#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850"],
                    data: [2478, 5267, 734, 784, 433]
                }
            ]
        },
        options: {
            legend: { display: false },
            title: {
                display: true,
                text: 'Predicted world population (millions) in 2050'
            }
        }
    });
}