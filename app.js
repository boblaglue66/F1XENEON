'use strict';
const services = {
  f1: { name: 'F1 TV', url: 'https://f1tv.formula1.com/' },
  canal: { name: 'CANAL+', url: 'https://www.canalplus.com/' }
};
const home = document.getElementById('home');
const viewer = document.getElementById('viewer');
const player = document.getElementById('player');
const reloadButton = document.getElementById('reloadButton');
let current = null;
function selectService(key) {
  const service = services[key];
  if (!service) return;
  current = service;
  document.getElementById('service').textContent = service.name;
  document.getElementById('external').href = service.url;
  player.title = 'Site officiel ' + service.name;
  player.src = service.url;
  home.hidden = true;
  viewer.hidden = false;
  reloadButton.hidden = false;
}
document.querySelectorAll('[data-service]').forEach(button => {
  button.addEventListener('click', () => selectService(button.dataset.service));
});
document.getElementById('homeButton').addEventListener('click', () => {
  player.src = 'about:blank';
  current = null;
  home.hidden = false;
  viewer.hidden = true;
  reloadButton.hidden = true;
  document.getElementById('service').textContent = 'CHOISIR UN SERVICE';
});
reloadButton.addEventListener('click', () => {
  if (current) player.src = current.url;
});
