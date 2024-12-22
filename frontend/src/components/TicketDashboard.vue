<template>
    <div class="flex flex-col lg:flex-row gap-6">
      <!-- Główna sekcja -->
      <div class="flex-1 bg-white shadow-lg p-6 rounded-md">
        <div class="mb-6">
          <h2 v-if="ticket" class="text-2xl font-bold mb-4">
            #{{ ticket.id }} - {{ ticket.title }}
          </h2>
          <p v-else class="text-gray-500">Loading ticket details...</p>
          <p v-if="ticket" class="text-gray-700">{{ ticket.description }}</p>
        </div>
  
        <div v-if="ticket">
          <h2 class="text-2xl font-bold mb-4">Messages</h2>
          <div
            v-for="(message, index) in ticket.messages"
            :key="index"
            class="mb-4 p-4 bg-gray-100 border border-gray-300 rounded-md"
          >
            <p class="text-gray-600">{{ message.content }}</p>
          </div>
  
          <!-- Formularz wiadomości -->
          <form @submit.prevent="sendMessage" class="space-y-4">
            <textarea
              v-model="newMessage"
              placeholder="Type your message..."
              class="w-full p-2 border border-gray-300 rounded-md resize-none"
            ></textarea>
            <div class="flex items-center gap-4">
              <input type="file" id="attachment" class="hidden" @change="handleAttachment" />
              <label for="attachment" class="cursor-pointer text-blue-500 hover:underline">
                Attach file
              </label>
              <span v-if="attachment" class="text-sm text-gray-500">{{ attachment.name }}</span>
            </div>
            <button
              type="submit"
              class="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
            >
              Send Message
            </button>
          </form>
        </div>
      </div>
  
      <!-- Panel boczny -->
      <TicketSidebar
        v-if="ticket"
        :ticket="ticket"
        :users="users"
        @applyChanges="applyChanges"
      />
    </div>
  </template>
  
  <script>
  import TicketSidebar from "./TicketSidebar.vue";
  
  export default {
    components: {
      TicketSidebar,
    },
    data() {
      return {
        ticket: null, // Dane ticketa
        users: [], // Lista użytkowników
        newMessage: "", // Nowa wiadomość
        attachment: null, // Załącznik
      };
    },
    mounted() {
      const ticketId = this.$route.params.id;
      this.fetchTicketDetails(ticketId);
      this.fetchUsers();
    },
    methods: {
      async fetchTicketDetails(id) {
        try {
          const response = await fetch(`http://localhost:5033/api/tickets/${id}/details`);
          if (!response.ok) throw new Error("Failed to fetch ticket details");
          this.ticket = await response.json();
        } catch (err) {
          console.error("Error fetching ticket details:", err);
        }
      },
      async fetchUsers() {
        try {
          const response = await fetch("http://localhost:5033/api/users");
          if (!response.ok) throw new Error("Failed to fetch users");
          this.users = await response.json();
        } catch (err) {
          console.error("Error fetching users:", err);
        }
      },
      sendMessage() {
        if (!this.newMessage) {
          alert("Message cannot be empty.");
          return;
        }
        console.log("Message sent:", this.newMessage, this.attachment);
        this.newMessage = "";
        this.attachment = null;
      },
      handleAttachment(event) {
        this.attachment = event.target.files[0];
      },
      applyChanges(updatedTicket) {
        console.log("Changes applied:", updatedTicket);
      },
    },
  };
  </script>
  
  <style scoped>
  .flex-1 {
    display: flex;
    flex-direction: column;
    gap: 1.5rem;
  }
  .bg-white {
    background-color: #ffffff;
  }
  .shadow-lg{
    box-shadow: 0 10px 15px rgba(0, 0, 0, 0.1);
}
</style>
  