import http from 'k6/http';
import { sleep } from 'k6';
import { check } from 'k6';

const token = "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6IjExMDMwNjAxOUBuY2N1LmVkdS50dyIsIlJvbGUiOiJOQ0NVU3R1ZGVudCIsIlVzZXJJZCI6IjYzODViMTcwNGI3OWM0MTZkNzZlZDQ4OCIsImV4cCI6MTcyNTcyOTQxNCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI0MyIsImF1ZCI6ImZyb250LWVuZC11cmwifQ.7-CD_uqKGQSo-O4i_533I5qaBkPHuI1BgIQKnUIO_lA";
const url_get20missions = "https://localhost:7243/nccupass/NormalTask/mainPage/20/0";

export const options = {
  stages: [
    // { duration: '0.017m', target: 4 },
    // { duration: '0.5m', target: 4 },
    // { duration: '1m', target: 0 },
    { duration: '1m', target: 10 }, // 1 minute at 10 VUs
    { duration: '5m', target: 50 }, // 5 minutes at 50 VUs
    { duration: '10m', target: 100 }, // 10 minutes at 100 VUs
    { duration: '5m', target: 0 }, // 5 minutes at 0 VUs (ramp down)
  ],
};

export default function () {
  const headers = {
    'accept': 'text/plain', // Update the accept header to 'application/json'
    'Authorization': token,
  };

  const res = http.get(url_get20missions, {headers});

  check(res, {
    'Status is 200': (r) => r.status === 200,
  });

  sleep(1);
}
