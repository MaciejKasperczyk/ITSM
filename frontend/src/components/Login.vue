<template>
  <div class="background-container">
    <div class="login-container">
      <div class="login-card">
        <h1 class="title">FixIT</h1>
        <p class="subtitle">Login to your account</p>
        <form @submit.prevent="login">
          <div class="form-group">
            <input
              type="text"
              id="username"
              v-model="credentials.username"
              placeholder="Username"
              required
              class="form-input"
            />
          </div>
          <div class="form-group">
            <input
              type="password"
              id="password"
              v-model="credentials.password"
              placeholder="Password"
              required
              class="form-input"
            />
          </div>
          <button type="submit" class="submit-button">Login</button>
        </form>
        <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "LoginForm",
  data() {
    return {
      credentials: {
        username: "",
        password: "",
      },
      errorMessage: null,
    };
  },
  methods: {
    async login() {
      try {
        const response = await axios.post(
          "http://localhost:5033/api/auth/login",
          this.credentials
        );
        localStorage.setItem("token", response.data.token);
        localStorage.setItem("username", response.data.username);
        alert("Login successful!");
        this.$router.push("/");
      } catch (error) {
        this.errorMessage =
          error.response?.data?.message || "Login failed. Try again!";
      }
    },
  },
};
</script>

<style>
/* Kontener z gradientem na pełnej szerokości i wysokości */
.background-container {
  width: 100%;
  height: 100vh;
  background: linear-gradient(135deg, #6e8efb, #a777e3);
  display: flex;
  justify-content: center;
  align-items: center;
}

/* Kontener logowania */
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  width: 100%;
}

/* Karta logowania */
.login-card {
  background: white;
  width: 100%;
  max-width: 400px;
  padding: 30px;
  border-radius: 10px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.2);
  text-align: center;
}

/* Tytuł aplikacji */
.title {
  font-size: 2rem;
  font-weight: bold;
  color: #4a4a4a;
  margin-bottom: 10px;
}

/* Podtytuł */
.subtitle {
  font-size: 1rem;
  color: #7a7a7a;
  margin-bottom: 20px;
}

/* Grupa formularza */
.form-group {
  margin-bottom: 20px;
}

/* Pole formularza */
.form-input {
  width: 100%;
  padding: 12px 15px;
  border: 1px solid #ccc;
  border-radius: 5px;
  font-size: 1rem;
  color: #4a4a4a;
  transition: all 0.3s ease-in-out;
}

.form-input:focus {
  border-color: #6e8efb;
  outline: none;
  box-shadow: 0 0 5px rgba(110, 142, 251, 0.5);
}

/* Przycisk */
.submit-button {
  width: 100%;
  padding: 12px 15px;
  background: linear-gradient(135deg, #6e8efb, #a777e3);
  color: white;
  font-size: 1rem;
  font-weight: bold;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  transition: all 0.3s ease-in-out;
}

.submit-button:hover {
  background: linear-gradient(135deg, #a777e3, #6e8efb);
}

/* Wiadomość o błędzie */
.error {
  margin-top: 15px;
  color: #ff6b6b;
  font-size: 0.9rem;
}
</style>
