let context = null;
let osc = null;
let oscType = null;
let oscFreq = null;

export function SetParameters(type = "sine", frequency = 440) {
    oscType = type ?? "sine";
    oscFreq = frequency ?? 440;
    if (osc !== null) {
        osc.type = oscType ?? "sine";
        osc.frequency.value = oscFreq ?? 440;
    }
}

export function StartPlaying() {
    // ReSharper disable once PossiblyUnassignedProperty
    context = new (window.AudioContext || window.webkitAudioContext)();
    osc = context.createOscillator();
    osc.type = oscType ?? "sine";
    osc.frequency.value = oscFreq ?? 440;
    osc.start();
    osc.connect(context.destination);
}

export function StopPlaying() {
    osc.stop(context.currentTime);
    osc.disconnect(context.destination);
    osc = null;
    context = null;
}

export function CopyTextToClipboard(textToCopy) {
    navigator.clipboard.writeText(textToCopy)
        .catch(function (error) {
            alert(error);
        });
}

export async function GetAudioOutputDevices() {
    let devices = [];

    devices = (await navigator.mediaDevices.enumerateDevices()).filter(function (entry) {
        return entry.kind === 'audiooutput' && entry.deviceId !== null && entry.deviceId !== "";
    });
    ;

    if (devices === null || devices.length === 0) {
        await navigator.mediaDevices.getUserMedia({audio: true});
        devices = (await navigator.mediaDevices.enumerateDevices()).filter(function (entry) {
            return entry.kind === 'audiooutput' && entry.deviceId !== null && entry.deviceId !== "";
        });
        ;
    }

    return devices;
}

export function SetAudioDevice(deviceId) {
    console.log(`setting device to ${deviceId}`)
    context.setSinkId(deviceId);
}
