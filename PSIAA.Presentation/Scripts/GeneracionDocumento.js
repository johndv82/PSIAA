function GroupByArrayObjects(_array) {
    var groupElements = Enumerable.From(_array)
        .GroupBy("$.Modelo", null,
                 function (key, g) {
                     return {
                         Modelo: key,
                         Detalle: Enumerable.From(g)
                            .GroupBy("$.Punto", null,
                                "{ Punto: $, Cantidad: $$.Sum('Number($.Cantidad)'), CostoSoles: $$.Sum('Number($.CostoSoles)'), CostoDolares: $$.Sum('Number($.CostoDolares)') }"
                            ).ToArray()
                     }
                 }).ToArray();
    return groupElements;
}

function ParseJsonToTable(_json) {
    //Redondear Costos Soles y Dolares
    for (var i in _json) {
        for (var j in _json[i]) {
            if (Array.isArray(_json[i][j])) {
                var det = _json[i][j];
                for (var d in det) {
                    det[d]['CostoSoles'] = Math.round(det[d]['CostoSoles'] * 100) / 100;
                    det[d]['CostoDolares'] = Math.round(det[d]['CostoDolares'] * 100) / 100;
                }
            }
        }
    }
    //console.log(_json);
    //Parsear Json aa una tabla HTML
    var col = [];
    for (var i = 0; i < _json.length; i++) {
        for (var key in _json[i]) {
            if (col.indexOf(key) === -1) {
                col.push(key);
            }
        }
    }

    var table = document.getElementById("dataTable");
    table.innerHTML = "";
    var tr = table.insertRow(-1);

    for (var i = 0; i < col.length; i++) {
        var th = document.createElement("th");
        th.innerHTML = col[i];
        tr.appendChild(th);
    }

    for (var i = 0; i < _json.length; i++) {
        tr = table.insertRow(-1);
        for (var j = 0; j < col.length; j++) {
            var tabCell = tr.insertCell(-1);

            var matriz = _json[i][col[j]];
            if (Array.isArray(matriz)) {
                //Nueva Tabla
                var col2 = [];
                //headers 2

                for (var ic = 0; ic < matriz.length; ic++) {
                    for (var key in matriz[ic]) {
                        if (col2.indexOf(key) === -1) {
                            col2.push(key);
                        }
                    }
                }

                var tb = document.createElement("table");
                tb.className = "table table-bordered";

                var tr2 = tb.insertRow(-1);

                for (var ih = 0; ih < col2.length; ih++) {
                    var th2 = document.createElement("th");
                    th2.innerHTML = col2[ih];
                    tr2.appendChild(th2);
                }
                ///data 2
                for (var ib = 0; ib < matriz.length; ib++) {

                    tr2 = tb.insertRow(-1);

                    for (var j = 0; j < col2.length; j++) {
                        var tabCell2 = tr2.insertCell(-1);
                        tabCell2.innerHTML = matriz[ib][col2[j]];
                    }
                }
                tabCell.appendChild(tb);

            } else {
                tabCell.innerHTML = _json[i][col[j]];
            }
        }
    }
}