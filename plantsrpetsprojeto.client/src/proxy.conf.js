const PROXY_CONFIG = [
  {
    context: [
      "/api", // Match all API routes
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
    target: "https://localhost:7024", // Match your backend port
    secure: false,
    changeOrigin: true,
    logLevel: "debug",
  }
];

module.exports = PROXY_CONFIG;
