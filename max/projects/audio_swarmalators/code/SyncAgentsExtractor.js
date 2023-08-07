inlets = 1;
outlets = 1;

var elementsPerObj = 6;

function list(val){
    var values = arguments;
    if(values.length >= elementsPerObj){//If list is elementsPerObj elements or more
        for (var i = 0; i < values.length; i += elementsPerObj) {
            if (i + elementsPerObj - 1 > values.length) break;
            var id = parseInt(values[i]);
			var phase =  parseInt(values[i + 1]) * 0.001/ (2 * Math.PI);
			var angle = parseInt(values[i + 2]) * 0.001;
            var x = parseInt(values[i + 3]) * 0.001;
            var y = parseInt(values[i + 4]) * 0.001;
            var z = parseInt(values[i + 5]) * 0.001;
            outlet(0, [id, phase, angle, x, y, z]);
        }                    
    }    
}