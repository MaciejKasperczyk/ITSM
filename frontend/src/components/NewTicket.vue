<template>
  <div class="max-w-3xl mx-auto bg-white p-6 rounded-md shadow-md">
    <h1 class="text-2xl font-bold mb-6">Create New Ticket</h1>
    <form @submit.prevent="createTicket">
      <div class="mb-4">
        <label class="block text-gray-700 font-bold mb-2">Title</label>
        <input
          v-model="ticket.title"
          type="text"
          class="w-full border-gray-300 rounded-md shadow-sm focus:ring focus:ring-blue-200"
          required
        />
      </div>
      <div class="mb-4">
        <label class="block text-gray-700 font-bold mb-2">Description</label>
        <textarea
          v-model="ticket.description"
          class="w-full border-gray-300 rounded-md shadow-sm focus:ring focus:ring-blue-200"
          required
        ></textarea>
      </div>
      <div class="mb-4">
        <label class="block text-gray-700 font-bold mb-2">Priority</label>
        <select
          v-model="ticket.priority"
          class="w-full border-gray-300 rounded-md shadow-sm focus:ring focus:ring-blue-200"
        >
          <option value="Low">Low</option>
          <option value="Medium">Medium</option>
          <option value="High">High</option>
        </select>
      </div>
      <div class="mb-4">
        <label class="block text-gray-700 font-bold mb-2">Type</label>
        <select
          v-model="ticket.type"
          class="w-full border-gray-300 rounded-md shadow-sm focus:ring focus:ring-blue-200"
        >
          <option value="0">Incident</option>
          <option value="1">Service Request</option>
        </select>
      </div>
      <button
        type="submit"
        class="bg-blue-600 text-white px-4 py-2 rounded shadow hover:bg-blue-500"
      >
        Create Ticket
      </button>
    </form>
  </div>
</template>

<script>
export default {
  data() {
    return {
      ticket: {
        title: "",
        description: "",
        priority: "Medium",
        type: 0,
        userId: null, // Ustawione na null do czasu pobrania użytkownika
      },
    };
  },
  async mounted() {
    await this.fetchCurrentUser();
  },
  methods: {
    async fetchCurrentUser() {
      try {
        const response = await fetch("http://localhost:5033/api/user/current");
        if (!response.ok) throw new Error("Failed to fetch current user");

        const user = await response.json();
        this.ticket.userId = user.id; // Ustawia userId na podstawie odpowiedzi API
      } catch (err) {
        console.error("Error fetching current user:", err);
        alert("Failed to fetch user. Please refresh the page.");
      }
    },
    async createTicket() {
      try {
        const response = await fetch("http://localhost:5033/api/tickets", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(this.ticket),
        });

        if (!response.ok) {
          const errorDetails = await response.json();
          console.error("Error details:", errorDetails);
          throw new Error("Failed to create ticket");
        }

        alert("Ticket created successfully!");
        this.$router.push("/");
      } catch (err) {
        console.error("Error creating ticket:", err);
        alert("Failed to create ticket.");
      }
    },
  },
};
</script>
