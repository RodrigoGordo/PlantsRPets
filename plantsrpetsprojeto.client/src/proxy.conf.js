const PROXY_CONFIG = [
  {
    context: [
      "/api", 
      "/login",
      "/logout",
      "/register",
      "/refresh",
      "/confirmEmail",
      "/resendConfirmationEmail",
      "/forgotPassword",
      "/resetPassword",
      "/manage/**"
    ],
    target: "https://localhost:7024",
    secure: false,
    changeOrigin: true,
    logLevel: "debug",
  }
];

module.exports = PROXY_CONFIG;
