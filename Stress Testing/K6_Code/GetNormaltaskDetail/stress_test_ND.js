import http from 'k6/http';
import { sleep, check} from 'k6';

const token = "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6IjExMDMwNjAxOUBuY2N1LmVkdS50dyIsIlJvbGUiOiJOQ0NVU3R1ZGVudCIsIlVzZXJJZCI6IjYzODViMTcwNGI3OWM0MTZkNzZlZDQ4OCIsImV4cCI6MTcyNTcyOTQxNCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI0MyIsImF1ZCI6ImZyb250LWVuZC11cmwifQ.7-CD_uqKGQSo-O4i_533I5qaBkPHuI1BgIQKnUIO_lA";
const url_getnormtaskdetail = "https://localhost:7243/nccupass/NormalTask/detail/64f6990db32d44ce65b9f07c";

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
  //1st vers (X: get request timeout after 16 min, goal is 21 min)
  // const res = http.get(url_getnormtaskdetail, {headers});
  //2nd vers (still have not tried)
  const res = http.get(url_getnormtaskdetail, { headers, timeout: '60s' });

  check(res, {
    'Status is 200': (r) => r.status === 200,
  });

  sleep(1);
}
