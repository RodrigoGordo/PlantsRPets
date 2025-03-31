importScripts('https://www.gstatic.com/firebasejs/10.7.1/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/10.7.1/firebase-messaging.js');

firebase.initializeApp({
  apiKey: "AIzaSyDXKHj5gPeT8plAxH2XMFmaLOBUbJQEuFI",
  authDomain: "plantsrpets-88284.firebaseapp.com",
  projectId: "plantsrpets-88284",
  storageBucket: "plantsrpets-88284.firebasestorage.app",
  messagingSenderId: "695166487704",
  appId: "1:695166487704:web:19208d177b39c8fad214ed",
});

const messaging = firebase.messaging();

messaging.onBackgroundMessage((payload) => {
  console.log('[firebase-messaging-sw.js] Received background message ', payload);
  const notificationTitle = payload.notification.title;
  const notificationOptions = {
    body: payload.notification.body,
    icon: '/firebase-logo.png'
  };

  self.registration.showNotification(notificationTitle, notificationOptions);
});
