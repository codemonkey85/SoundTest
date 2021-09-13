var context = null;
var osc = null;

var oscType = null;
var oscFreq = null;

export function initializeSoundGenerator() {
    context = new (window.AudioContext || window.webkitAudioContext)();
}

export function setParameters(type = 'sine', frequency = 440) {
    oscType = type ?? 'sine';
    oscFreq = frequency ?? 440;
    if (osc !== null) {
        osc.type = oscType ?? 'sine';
        osc.frequency.value = oscFreq ?? 440;
    }
}

export function startPlaying() {
    osc = context.createOscillator();
    osc.type = oscType ?? 'sine';
    osc.frequency.value = oscFreq ?? 440;
    osc.start();
    osc.connect(context.destination);
}

export function stopPlaying() {
    osc.stop(context.currentTime);
    osc.disconnect(context.destination);
    osc = null;
}
