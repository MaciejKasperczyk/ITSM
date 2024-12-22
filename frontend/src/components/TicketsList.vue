<template>
  <div class="p-6 bg-white shadow-md rounded-md">
    <h1 class="text-2xl font-bold mb-6">Tickets</h1>

    <TicketFilters
      :users="users"
      :initialFilters="filters"
      @filter-changed="applyFilters"
    />

    <table class="min-w-full bg-white border border-gray-300 rounded-md shadow-md">
      <thead class="bg-gray-100">
        <tr>
          <th class="py-2 px-4 text-left font-semibold">#</th>
          <th class="py-2 px-4 text-left font-semibold">Title</th>
          <th class="py-2 px-4 text-left font-semibold">Status</th>
          <th class="py-2 px-4 text-left font-semibold">Priority</th>
          <th class="py-2 px-4 text-left font-semibold">Created By</th>
          <th class="py-2 px-4 text-left font-semibold">Assigned IT</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="ticket in filteredTickets"
          :key="ticket.id"
          class="border-t hover:bg-gray-100 cursor-pointer"
          @click="navigateToTicket(ticket.id)"
        >
          <td class="py-2 px-4">{{ ticket.id }}</td>
          <td class="py-2 px-4">{{ ticket.title }}</td>
          <td class="py-2 px-4">
            <span
              :class="statusClasses(ticket.status)"
              class="px-2 py-1 rounded-full text-xs font-semibold"
            >
              {{ ticket.status }}
            </span>
          </td>
          <td class="py-2 px-4">{{ ticket.priority }}</td>
          <td class="py-2 px-4">{{ ticket.user?.fullName || "Unknown" }}</td>
          <td class="py-2 px-4">{{ ticket.assignedTo?.fullName || "Unassigned" }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import TicketFilters from "./TicketFilters.vue";

export default {
  components: { TicketFilters },
  data() {
    return {
      tickets: [],
      users: [],
      filters: {
        status: "",
        priority: "",
        assignedIt: "",
      },
    };
  },
  computed: {
    filteredTickets() {
      return this.tickets.filter((ticket) => {
        return (
          (!this.filters.status || ticket.status === this.filters.status) &&
          (!this.filters.priority || ticket.priority === this.filters.priority) &&
          (!this.filters.assignedIt || ticket.assignedTo?.fullName === this.filters.assignedIt)
        );
      });
    },
  },
  mounted() {
    this.fetchTickets();
    this.fetchUsers();
  },
  methods: {
    async fetchTickets() {
      try {
        const response = await fetch("http://localhost:5033/api/tickets");
        if (!response.ok) throw new Error("Failed to fetch tickets");
        this.tickets = await response.json();
      } catch (err) {
        console.error("Error fetching tickets:", err);
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
    applyFilters(newFilters) {
      this.filters = newFilters;
    },
    navigateToTicket(id) {
      this.$router.push(`/tickets/${id}/details`);
    },
    statusClasses(status) {
      switch (status) {
        case "Open":
          return "bg-blue-200 text-blue-800";
        case "In Progress":
          return "bg-yellow-200 text-yellow-800";
        case "Resolved":
          return "bg-green-200 text-green-800";
        case "Closed":
          return "bg-red-200 text-red-800";
        default:
          return "bg-gray-200 text-gray-800";
      }
    },
  },
};
</script>
