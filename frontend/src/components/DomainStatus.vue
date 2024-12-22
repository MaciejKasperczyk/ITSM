<template>
  <div>
    <span
      :class="{
        'text-green-500': status === 'Connected',
        'text-red-500': status === 'Disconnected',
      }"
    >
      ● {{ status }}
    </span>
  </div>
</template>

<script>
export default {
  data() {
    return {
      status: "Checking...",
    };
  },
  async mounted() {
    try {
      const response = await fetch(
        "http://localhost:5033/api/auth/domain-status?username=test&password=1234"
      );

      const data = await response.json();
      this.status = data.Status || "Disconnected";
    } catch (error) {
      console.error("Failed to check domain status:", error);
      this.status = "Disconnected";
    }
  },
};
</script>
