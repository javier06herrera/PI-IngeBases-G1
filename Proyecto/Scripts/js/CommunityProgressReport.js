function initReport()
{
	// Obtain dropdown objects.
	//var memberRankDropdown = document.getElementById("memberRanks");
	var filtersDropdown = document.getElementById("filters");
	// Obtain selected items in dropdowns.
	//var selectedMemberRanks = getSelectedItems(memberRankDropdown);
	var selectedFilters = getSelectedItems(filtersDropdown);
	// Generate report according to selected items.
	//generateReport(selectedMemberRanks, selectedFilters);
	generateReport(selectedFilters);
}

function getSelectedItems(dropdownlist)
{
	// Return an array of strings containing the selected options of the given dropdownlist.
	var selectedItems = [];
	var len = dropdownlist.options.length;
	var currentOption = null;

	for (var item = 0; item < len; ++item) {
		currentOption = dropdownlist.options[item];
		if (currentOption.selected) {
			selectedItems.push(currentOption.value);
		}
	}
	return selectedItems;
}

//function generateReport(selectedMemberRanks, selectedFilters)
//{
//    var values = null;
//    var canvas = null;
//    var row = document.getElementById("row");
//    row.innerHTML = "";

//    // For each filter, get the required data, create a canvas and graphicate it.
//    for (var filter = 0; filter < selectedFilters.length; ++filter) {
//        values = getFilteredValues(selectedMemberRanks, selectedFilters[filter]);
//        canvas = createCanvas();
//        drawGraphics(canvas, values);
//    }
//}

function generateReport(selectedFilters) {
	var values = null;
	var canvas = null;
	var row = document.getElementById("row");
	row.innerHTML = "";
	var toGraphicate = null;
	// For each filter, get the required data, create a canvas and graphicate it.
	for (var filter = 0; filter < selectedFilters.length; ++filter) {
		if (selectedFilters[filter] == "Number of articles peer category and topic" || selectedFilters[filter] == "Access Count peer category and topic") {
			values = getFilteredTable(selectedFilters[filter]);
			canvas = createCanvas();
			toGraphicate = extractDataSet(values);
			drawStackedColumns(canvas, toGraphicate[0], toGraphicate[1], toGraphicate[2], selectedFilters[filter]);
		}
		else {
			values = getFilteredValues(selectedFilters[filter]);
			canvas = createCanvas();
			toGraphicate = getValuesAndLabels(values);
			drawColumns(canvas, toGraphicate[0], toGraphicate[1], "Gráfico");
		}
		//drawGraphics(canvas, values);        
	}
}

function extractDataSet(values) {
	var result = [];
	label = [];
	members = [];
	for (var columns = 1; columns < values[0].length; ++columns) {
		if (values[1][columns] != "#") {
			group = [];            
			for (var rows = 1; rows < values.length; ++rows) {
				group.push(parseInt(values[rows][columns]))

			}
			label.push(values[0][columns])
			result.push(group)
		}
	}
	for (var rows = 1; rows < values.length; ++rows) {
		members.push(values[rows][0]);
	}
	return [result, label, members];
}


//Glori
//function getFilteredValues(selectedMemberRanks, filter)
//{
//    var filteredValues = null;
//    $.ajax({
//        url: '/CommunityProgressReport/GetFilteredValues',
//        data: {
//            selectedMemberRanks: selectedMemberRanks,
//            filter: filter,
//        },
//        type: 'post',
//        dataType: 'json',
//        async: false,
//        success: function (results) {
//            filteredValues = results;
//        }

//    });
//    return filteredValues;
//}

//Nueva sin miembros
function getFilteredValues(filter) {
	var filteredValues = null;
	$.ajax({
		url: '/CommunityProgressReport/GetFilteredValues',
		data: {
			filter: filter,
		},
		type: 'post',
		dataType: 'json',
		async: false,
		success: function (results) {
			filteredValues = results;
		}

	});
	return filteredValues;
}

function getFilteredTable(filter) {
	var filteredValues = null;
	$.ajax({
		url: '/CommunityProgressReport/GetFilteredTable',
		data: {
			filter: filter,
		},
		type: 'post',
		dataType: 'json',
		async: false,
		success: function (results) {
			filteredValues = results;
		}

	});
	return filteredValues;
}

function createCanvas()
{
	// Generate required html elements inside a predefined row, in order to place a canvas. 
	var row = document.getElementById("row");
	var colDiv = document.createElement("div");
	colDiv.setAttribute("class", "col-md-6 py-1");
	var cardDiv = document.createElement("div");
	cardDiv.setAttribute("class", "card");
	var cardBodyDiv = document.createElement("div");
	cardBodyDiv.setAttribute("class", "card-body");
	var canvas = document.createElement("canvas");
	cardBodyDiv.appendChild(canvas);
	cardDiv.appendChild(cardBodyDiv);
	colDiv.appendChild(cardDiv);
	row.appendChild(colDiv);
	return canvas;
}

function drawGraphics(canvas, values)
{
	new Chart(canvas, {
		type: 'doughnut',
		data: {
			labels: ["Africa", "Asia", "Europe", "Latin America", "North America"],
			datasets: [
				{
					label: "Population (millions)",
					backgroundColor: ["#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850"],
					data: values,
				}
			]
		},
		options: {
			legend: { display: false },
			title: {
				display: true,
				text: 'Predicted world population (millions) in 2050'
			},
			responsive: true,
			maintainAspectRatio: true,
			devicePixelRatio: 2
		}
	});
}

//Functions for user report
function initReportUser() {
	// Obtain dropdown objects.
	var categoriesDropdown = document.getElementById("categories");
	// Obtain selected items in dropdowns.
	var selectedCategories = getSelectedItems(categoriesDropdown);
	if (selectedCategories.length > 0) {
		generateReportUser(selectedCategories);
	}
	else {
		alert("Please select an option!");
	}
}

function generateReportUser(selectedCategories) {
	var values = null;
	var canvas = null;
	var title = null
	var toGraphicate = null;
	var row = document.getElementById("row");
	row.innerHTML = "";
	// For each filter, get the required data, create a canvas and graphicate it.
	for (var categories = 0; categories < selectedCategories.length; ++categories) {
		values = getFilteredValuesUser(selectedCategories[categories]);
		title = "Distribution of " + selectedCategories[categories].toLowerCase() + " for community members";
		toGraphicate = getValuesAndLabels(values);
		canvas = createCanvas();
		drawColumns(canvas, toGraphicate[0], toGraphicate[1], title);
	}
}

function getFilteredValuesUser(category) {
	var categories = null;
	var jsonString = null;
	$.ajax({
		url: '/Report/ReportUsers',
		data: {
			category: category,
		},
		type: 'post',
		dataType: 'json',
		async: false,
		success: function (results) {
			categories = results;
		}

	});
	return categories;
}

function getValuesAndLabels(jsonElements) {
	var values = []
	var labels = []
	for (var item = 0; item < jsonElements.length; ++item) {
		values.push(jsonElements[item].y)
		labels.push(jsonElements[item].label)
	}
	return [values, labels]
}

function drawColumns(canvas, values, label, title) {

	var limit = values.length;
	var dataP = [];
	for (var i = 0; i < limit; i++) {
		dataP.push({ y: values[i], label: label[i] });
	}

	new Chart(canvas, {
		type: 'bar',
		data: {

			labels: label,
			datasets: [
				{
					
					label: title,
					backgroundColor: ["#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850"],
					data: values
				}
			]
		},
		options: {
			scales: {
				yAxes: [{
					ticks: {
						beginAtZero: true
					}
				}]
			}
		},
	});
}

function drawStackedColumns(canvas, values, label, members, title)
{
	var limit = values.length;
	var dataP = [];
	for (var i = 0; i < limit; i++) {
		dataP.push({ type: "stackedcolumn", name: label[i], showInLegend : "true", datapoints: values[i]  });
	}

	//var chart = new CanvasJS.Chart("chartContainer", {
	//	animationEnabled: true,
	//	title: {
	//		text: "Demographics of a High School Marching Band"
	//	},
	//	toolTip: {
	//		shared: true
	//	},
	//	axisY: {
	//		title: "No. of Students"
	//	},
	//	legend: {
	//		cursor: "pointer",
	//		verticalAlign: "center",
	//		horizontalAlign: "right",
	//		itemclick: toggleDataSeries
	//	},
	//	data: {
	//		datasets = dataP,
	//		labels = members,
	//	}		
		
	//});

	//new Chart(canvas, {
	//	animationEnabled: true,
	//	title: {
	//		text: "Demographics of a High School Marching Band"
	//	},
	//	toolTip: {
	//		shared: true
	//	},
	//	axisY: {
	//		title: "No. of Students"
	//	},
	//	//legend: {
	//	//	cursor: "pointer",
	//	//	verticalAlign: "center",
	//	//	horizontalAlign: "right",
	//	//	itemclick: toggleDataSeries
 // //          //itemclick: ["prueba 1", "prueba2"]
 // //      },
 //       data:
	//	//data: dataP,
 //       //data: 
 //           [{
 //               type: "stackedColumn",
 //               showInLegend: true,
 //               color: "#696661",
 //               name: "Q1",
 //               dataPoints: [
 //                   { y: 6.75, x: 3 },
 //                   { y: 8.57, x: 2 },
 //                   { y: 10.64, x: 4 },
 //                   { y: 13.97, x: 5 },
 //                   { y: 15.42, x: 6 },
 //                   { y: 17.26, x: 7 },
 //                   { y: 20.26, x: 8 }
 //               ]


 //           }]
 //       //}],
 //   });

    //new Chart(canvas, {
    //    animationEnabled: true,
    //    title: {
    //        text: "Google - Consolidated Quarterly Revenue",
    //        fontFamily: "arial black",
    //        fontColor: "#695A42"
    //    },

    //    axisY: {
    //        valueFormatString: "$#0bn",
    //        gridColor: "#B6B1A8",
    //        tickColor: "#B6B1A8"
    //    },
    //    toolTip: {
    //        shared: true,
    //        content: toolTipContent
    //    },
    //    data: [{
    //        type: "stackedColumn",
    //        showInLegend: true,
    //        color: "#696661",
    //        name: "Q1",
    //        dataPoints: [
    //            { y: 6.75, x: 3 },
    //            { y: 8.57, x: 2 },
    //            { y: 10.64, x: 4 },
    //            { y: 13.97, x: 5 },
    //            { y: 15.42, x: 6 },
    //            { y: 17.26, x: 7 },
    //            { y: 20.26, x: 8 }
    //        ]


    //    }]
    //});

    //new Chart(canvas, {
    //    type: 'bar',
    //    data: {

    //        labels: label,
    //        datasets: [
    //            {

    //                label: members,
    //                backgroundColor: ["#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850"],
    //                data: values[0]
    //            }
    //        ]
    //    },
    //    options: {
    //        scales: {
    //            yAxes: [{
    //                ticks: {
    //                    beginAtZero: true
    //                }
    //            }]
    //        }
    //    },
    //});

    //new Chart(canvas,
    //    {
    //        title: {
    //            text: "Evening Sales"
    //        },
    //        axisX: {
    //            valueFormatString: "string",
    //        //    interval: 1,
    //        //    intervalType: "month"

    //        },
    //        data: [
    //            {
    //                type: "stackedColumn",
    //                legendText: "meals",
    //                showInLegend: "true",
    //                dataPoints: [
    //                    { label: "String 1", y: 55 },
    //                    { label: "String 2", y: 50 },
    //                    { label: "String 3", y: 65 },
    //                    { label: "String 4", y: 95 },
    //                    { label: "String 5", y: 71 }

    //                ]
    //            },
    //            {
    //                type: "stackedColumn",
    //                legendText: "snacks",
    //                showInLegend: "true",
    //                dataPoints: [
    //                    { label: "String 1", y: 71 },
    //                    { label: "String 2", y: 55 },
    //                    { label: "String 3", y: 50 },
    //                    { label: "String 4", y: 65 },
    //                    { label: "String 5", y: 95 }

    //                ]
    //            },
    //            {
    //                type: "stackedColumn",
    //                legendText: "Drinks",
    //                showInLegend: "true",
    //                dataPoints: [
    //                    { label: "String 1", y: 71 },
    //                    { label: "String 2", y: 55 },
    //                    { label: "String 3", y: 50 },
    //                    { label: "String 4", y: 65 },
    //                    { label: "String 5", y: 95 }

    //                ]
    //            },

    //            {
    //                type: "stackedColumn",
    //                legendText: "dessert",
    //                showInLegend: "true",
    //                dataPoints: [
    //                    { label: "String 1", y: 61 },
    //                    { label: "String 2", y: 75 },
    //                    { label: "String 3", y: 80 },
    //                    { label: "String 4", y: 85 },
    //                    { label: "String 5", y: 105 }

    //                ]
    //            },
    //            {
    //                type: "stackedColumn",
    //                legendText: "pick-ups",
    //                showInLegend: "true",
    //                dataPoints: [
    //                    { label: "String 1", y: 20 },
    //                    { label: "String 2", y: 35 },
    //                    { label: "String 3", y: 30 },
    //                    { label: "String 4", y: 45 },
    //                    { label: "String 5", y: 25 }

    //                ]
    //            }

    //        ]
    //    });

   


    //new Chart(canvas, {
    //    type: 'bar',
    //    data:
    //        //[
    //        //    {
    //        //        type: "stackedColumn",
    //        //        legendText: "meals",
    //        //        showInLegend: "true",
    //        //        dataPoints: [
    //        //            { label: "String 1", y: 55 },
    //        //            { label: "String 2", y: 50 },
    //        //            { label: "String 3", y: 65 },
    //        //            { label: "String 4", y: 95 },
    //        //            { label: "String 5", y: 71 }

    //        //        ]
    //        //},
    //        //],
    //        [{
    //            type: "stackedColumn",
    //            showInLegend: true,
    //            color: "#696661",
    //            name: "Q1",
    //            dataPoints: [
    //                { y: 6.75, x: 3 },
    //                { y: 8.57, x: 2 },
    //                { y: 10.64, x: 4 },
    //                { y: 13.97, x: 5 },
    //                { y: 15.42, x: 6 },
    //                { y: 17.26, x: 7 },
    //                { y: 20.26, x: 8 }
    //            ]


    //        }],
    //    options: {
    //        scales: {
    //            axisX: [{
                    
    //            }],
    //            yAxes: [{
    //                stacked: true
    //            }]
    //        }
    //    }
    //});

    var chart = new CanvasJS.Chart(canvas, {
        animationEnabled: true,
        title: {
            text: "Google - Consolidated Quarterly Revenue",
            fontFamily: "arial black",
            fontColor: "#695A42"
        },

        axisY: {
            valueFormatString: "$#0bn",
            gridColor: "#B6B1A8",
            tickColor: "#B6B1A8"
        },        
        data: [{
            type: "stackedColumn",
            showInLegend: true,
            color: "#696661",
            name: "Q1",
            dataPoints: [
                { y: 6.75, x: 3 },
                { y: 8.57, x: 2 },
                { y: 10.64, x: 4 },
                { y: 13.97, x: 5 },
                { y: 15.42, x: 6 },
                { y: 17.26, x: 7 },
                { y: 20.26, x: 8 }
            ]


        }]
    });

    chart.render();
    //chart.options.data.

}