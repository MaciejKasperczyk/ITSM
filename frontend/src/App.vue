<template>
  <div class="h-screen flex flex-col">
    <!-- Warunkowe renderowanie -->
    <div v-if="showMenu" class="flex h-full">
      <!-- Panel boczny -->
      <aside class="w-64 bg-gray-800 text-white flex flex-col h-full">
        <div class="p-4 text-2xl font-bold border-b border-gray-700">FixIT</div>
        <nav class="flex-1 p-4 space-y-4">
          <router-link
            to="/"
            class="block px-4 py-2 rounded hover:bg-gray-700"
            active-class="bg-gray-700"
          >
            Tickets
          </router-link>
          <router-link
            to="/users"
            class="block px-4 py-2 rounded hover:bg-gray-700"
            active-class="bg-gray-700"
          >
            Users
          </router-link>
          <router-link
            to="/settings"
            class="block px-4 py-2 rounded hover:bg-gray-700"
            active-class="bg-gray-700"
          >
            Settings
          </router-link>
        </nav>
      </aside>

      <!-- Główna zawartość -->
      <div class="flex-1 flex flex-col h-full">
        <!-- Pasek górny -->
        <header class="bg-white shadow p-4 flex justify-between items-center">
          <h1 class="text-xl font-semibold">FixIT - 1.0.0</h1>
          <div class="flex items-center space-x-6">
            <!-- Wskaźnik statusu domeny -->
            <div class="flex items-center space-x-2">
              <div
                :class="domainStatusClass"
                class="h-4 w-4 rounded-full"
                :title="domainStatusMessage"
              ></div>
              <span class="text-sm font-medium">
                {{ domainStatusMessage }}
                <template v-if="loggedInUser">- {{ loggedInUser }}</template>
              </span>
            </div>

            <!-- Dodawanie nowego zgłoszenia -->
            <router-link
              to="/tickets/new"
              class="text-white bg-blue-600 px-4 py-2 rounded shadow hover:bg-blue-500"
            >
              + Add Ticket
            </router-link>

            <!-- Wylogowanie -->
            <button
              @click="logout"
              class="text-white bg-red-600 px-4 py-2 rounded shadow hover:bg-red-500"
            >
              Logout
            </button>
          </div>
        </header>

        <!-- Dynamiczna zawartość -->
        <main class="flex-1 p-6 overflow-auto bg-gray-100">
          <router-view />
        </main>
      </div>
    </div>

    <!-- Widok bez menu (np. logowanie) -->
    <div v-else class="h-full flex items-center justify-center bg-gray-100">
      <router-view />
    </div>
  </div>
</template>

<script>
export default {
  name: "App",
  data() {
    return {
      domainStatus: "Disconnected", // Domyślny status
      loggedInUser: null, // Email zalogowanego użytkownika
    };
  },
  computed: {
    showMenu() {
      return this.$route.path !== "/login";
    },
    domainStatusClass() {
      return this.domainStatus === "Connected" ? "bg-green-500" : "bg-red-500";
    },
    domainStatusMessage() {
      return this.domainStatus === "Connected"
        ? "Domain Connected"
        : "Domain Disconnected";
    },
  },
  mounted() {
    this.checkDomainStatus();
    this.loadLoggedInUser();
  },
  methods: {
    async checkDomainStatus() {
      try {
        const response = await fetch("http://localhost:5033/api/domain/status");
        const data = await response.json();
        if (response.ok) {
          this.domainStatus = data.status || "Disconnected";
        } else {
          console.error("Failed to fetch domain status:", data.error);
        }
      } catch (err) {
        console.error("Error checking domain status:", err);
      }
    },
    loadLoggedInUser() {
      const username = localStorage.getItem("username");
      if (username) {
        this.loggedInUser = username;
      } else {
        this.loggedInUser = null;
      }
    },
    logout() {
      // Wyczyszczenie lokalnej pamięci
      localStorage.removeItem("token");
      localStorage.removeItem("username");
      this.loggedInUser = null;

      // Przekierowanie na stronę logowania
      this.$router.push("/login");
    },
  },
  watch: {
    // Watcher na zmianę trasy, aby za każdym razem sprawdzić użytkownika
    $route() {
      this.loadLoggedInUser();
    },
  },
};
</script>

<style>
html,
body,
#app {
  height: 100%;
  margin: 0;
}
</style>
