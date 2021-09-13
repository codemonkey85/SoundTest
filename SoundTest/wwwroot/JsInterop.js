// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

export function generateSound(type = 'sine', frequency = 440, stopTime = 1) {
    var context = new (window.AudioContext || window.webkitAudioContext)();
    var osc = context.createOscillator(); // instantiate an oscillator
    osc.type = type ?? 'sine'; // this is the default - also square, sawtooth, triangle
    osc.frequency.value = frequency ?? 440; // Hz
    osc.connect(context.destination); // connect it to the destination
    osc.start(); // start the oscillator
    osc.stop(context.currentTime + stopTime ?? 1); // stop 2 seconds after the current time
}
