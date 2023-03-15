var context = null;
var osc = null;
var oscType = null;
var oscFreq = null;

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
    navigator.clipboard.writeText(textToCopy).then(function () {
        alert("Copied to clipboard!");
    })
        .catch(function (error) {
            alert(error);
        });
}

export async function GetAudioOutputDevices() {
    var devices = [];
    var audioOutputDevices = [];

    devices = await navigator.mediaDevices.enumerateDevices()
    devices.forEach(device => {
        if (device.kind === 'audiooutput') // && device.deviceId !== 'default' && device.deviceId !== 'communications')
            //console.log(device);
            audioOutputDevices.push({
                "deviceId": device.deviceId,
                "label": device.label,
                "groupId": device.groupId
            });
    });

    return audioOutputDevices;
}

export function SetAudioDevice(deviceId) {
    console.log(`setting device to ${deviceId}`)
    context.setSinkId(deviceId);
}
